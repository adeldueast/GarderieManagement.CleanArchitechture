﻿using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GarderieManagementClean.Application.Interfaces.Services;
using Contracts.Dtos.Request;
using Contracts.Request;
using System.Diagnostics;
using GarderieManagementClean.Domain.Entities;

namespace GarderieManagementClean.Infrastructure.Identity
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IEmailService _emailService;

        public IdentityRepository(
                IEmailService emailService,
             UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager,
             JwtSettings jwtSettings,
             ApplicationDbContext context, TokenValidationParameters tokenValidationParameters)
        {
            _emailService = emailService;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            // _logger = logger;
        }


        public async Task<Result<Authentication>> RegisterOwnerAsync(UserRegistrationRequest userRegistrationRequest)
        {

            // we have to manullay check if email is null Or empty because we actually allow certain users with no email (tutors/guardians)

            if (string.IsNullOrEmpty(userRegistrationRequest.Email))
            {
                return new Result<Authentication>()
                {
                    Success = false,
                    Errors = new List<string>() { $"Couldn't create user because email was not provided" }
                };
            }

            var newUser = new ApplicationUser
            {
                FirstName = userRegistrationRequest.FirstName,
                LastName = userRegistrationRequest.LastName,
                Email = userRegistrationRequest.Email,
                UserName = userRegistrationRequest.Email,
            };
            var roleExist = await _roleManager.RoleExistsAsync("owner");
            if (!roleExist)
            {
                return new Result<Authentication>()
                {
                    Success = false,
                    Errors = new List<string>() { $"Couldn't create user because role 'owner' doesn't exist." }
                };

            }

            var createdUser = await _userManager.CreateAsync(newUser, userRegistrationRequest.Password);

            if (!createdUser.Succeeded)
            {
                return new Result<Authentication>
                {
                    Errors = createdUser.Errors.Select(err => err.Description)
                };
            };

            //Assign role to newUser
            await _userManager.AddToRoleAsync(newUser, "owner");


            //TODO: Send email confirmation to user
            // var EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            // EmailConfirmationToken = WebUtility.UrlEncode(EmailConfirmationToken);
            // await _emailService.SendEmailAsync(newUser.Email, "Email confirmation", EmailConfirmationToken, newUser.Id);


            //return new Result<Authentication>
            //{
            //    Success = true,
            //    Data = new
            //    {
            //        Message = "Registrated successfuly"
            //        //Message = "Registrated successfuly, please confirm your email.",
            //        //UserId = newUser.Id,
            //        //ConfirmEmailToken = EmailConfirmationToken
            //    }
            //};


            return await GenerateAuthResult(newUser);



        }

        public async Task<Result<object>> InviteUser(string userId, UserInviteUserRequest inviteUserRequest)
        {
            // we have to manullay check if email is null Or empty because we actually allow certain users with no email (tutors/guardians)
            if (string.IsNullOrEmpty(inviteUserRequest.Email))
            {
                return new Result<object>()
                {
                    Success = false,
                    Errors = new List<string>() { $"Couldn't invite user because email was not provided" }
                };
            }

            var loggedInUser = await _userManager.FindByIdAsync(userId);
            if (loggedInUser.GarderieId is null)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"Failed to invite user '{inviteUserRequest.Email}', because there is no existing garderie. Create one before you invite users." }
                };
            }

            //check if email is already used
            var user = await _userManager.FindByEmailAsync(inviteUserRequest.Email);
            if (user != null)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"Failed to invite user '{inviteUserRequest.Email}', because email is already used." }
                };
            }


            string[] roles = { "admin", "employee" };

            //check if role is a valid Role (exist)
            foreach (var role in roles)
            {


                var roleResult = await _roleManager.RoleExistsAsync(role);

                if (!roleResult)
                {
                    return new Result<object>()
                    {
                        Errors = new List<string>() { $"Failed to invite user '{inviteUserRequest.Email}', because Role '{role}' does not exist." }
                    };
                }
            }





            var newUser = new ApplicationUser
            {
                FirstName = inviteUserRequest.FirstName,
                LastName = inviteUserRequest.LastName,
                Email = inviteUserRequest.Email,
                UserName = inviteUserRequest.Email,
                GarderieId = loggedInUser.GarderieId,
                Phone = inviteUserRequest.Phone
            };

            //Create new password-less User
            var createdUser = await _userManager.CreateAsync(newUser, "password");
            if (!createdUser.Succeeded)
            {
                return new Result<object>
                {
                    Errors = createdUser.Errors.Select(err => err.Description)
                };
            };

            if (inviteUserRequest.isAdmin)
            {
                await _userManager.AddToRoleAsync(newUser, "admin");

            }
            await _userManager.AddToRoleAsync(newUser, "employee");



            //TODO: Send invite confirmation to user
            // var EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            // EmailConfirmationToken = WebUtility.UrlEncode(EmailConfirmationToken);
            // await _emailService.SendEmailAsync(newUser.Email, "Email confirmation", EmailConfirmationToken, newUser.Id);

            return new Result<object>
            {
                Success = true,
                Data = new
                {
                    Message = $"User {inviteUserRequest.Email} invited successfully ",
                    //Message = "Invitation successful, user needs to accept the invite.",
                    //UserId = newUser.Id,
                    //ConfirmEmailToken = EmailConfirmationToken
                }
            };



        }

        public async Task<Result<object>> InviteTutor(string userId, UserInviteTutorRequest inviteUserRequest)
        {

            var currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser.GarderieId is null)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"Failed to invite tutor '{inviteUserRequest.Email}', because there is no existing garderie. Create one before you invite users." }
                };
            }

            //check if child exist
            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.GarderieId == currentUser.GarderieId && e.Id == inviteUserRequest.EnfantId);
            if (enfant == null)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"Failed to invite user '{inviteUserRequest.Email}', because enfant '{inviteUserRequest.EnfantId}' is already used." }
                };

            }

            //check if email is already used
            //var user = await _userManager.FindByEmailAsync(inviteUserRequest.Email);
            //if (user != null)
            //{
            //    return new Result<object>()
            //    {
            //        Errors = new List<string>() { $"Failed to invite user '{inviteUserRequest.Email}', because email is already used." }
            //    };
            //}


            //check if role is a valid Role (exist)
            string role = "tutor";
            var roleResult = await _roleManager.RoleExistsAsync(role);
            if (!roleResult)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"Failed to invite user '{inviteUserRequest.Email}', because role '{role}' does not exist." }
                };
            }


            var hasAccount = inviteUserRequest.HasAnAccount ? true : false;
            var newUser = new ApplicationUser
            {
                FirstName = inviteUserRequest.FirstName,
                LastName = inviteUserRequest.LastName,
                Email = hasAccount
                            ? inviteUserRequest.Email
                            : null,
                UserName = hasAccount
                            ? inviteUserRequest.Email
                            : null,
                GarderieId = currentUser.GarderieId,

                hasAccount = inviteUserRequest.HasAnAccount ? true : false

            };


            //TODO: implement a email invitation to choose their own password

            var createdUser = await _userManager.CreateAsync(newUser, inviteUserRequest.HasAnAccount ? "password" : Guid.NewGuid().ToString());
            if (!createdUser.Succeeded)
            {
                return new Result<object>
                {
                    Errors = createdUser.Errors.Select(err => err.Description)
                };
            };

            //Assign role to user
            await _userManager.AddToRoleAsync(newUser, role);

            //Create relation between Tutor and Enfant
            newUser.Tutors.Add(
                new TutorEnfant
                {
                    ApplicationUser = newUser,
                    Enfant = enfant,
                    Relation = inviteUserRequest.Relation,
                    EmergencyContact = inviteUserRequest.EmergencyContact,
                    AuthorizePickup = inviteUserRequest.AuthorizePickup,
                });





            await _context.SaveChangesAsync();
            //TODO: Send invite confirmation to user
            // var EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            // EmailConfirmationToken = WebUtility.UrlEncode(EmailConfirmationToken);
            // await _emailService.SendEmailAsync(newUser.Email, "Email confirmation", EmailConfirmationToken, newUser.Id);

            return new Result<object>
            {
                Success = true,
                Data = new
                {
                    Message = $"User {inviteUserRequest.Email} invited successfully and assigned to '{inviteUserRequest.EnfantId}'",
                    //Message = "Invitation successful, user needs to accept the invite.",
                    //UserId = newUser.Id,
                    //ConfirmEmailToken = EmailConfirmationToken
                }
            };



        }


        public async Task<Result<Authentication>> AuthenticateAsync(UserLoginRequest userLoginRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(userLoginRequest.Email);

            if (existingUser == null)
            {
                return new Result<Authentication>()
                {
                    //Todo: be less specific
                    Errors = new[] { "Wrong email" }
                };
            }

            //var emailConfirmed = await _userManager.IsEmailConfirmedAsync(existingUser);

            //if (!emailConfirmed)
            //{
            //    return new Result<Authentication>()
            //    {
            //        Errors = new[] { "Please verify your email first" }
            //    };
            //}

            var hasPassword = await _userManager.HasPasswordAsync(existingUser);

            if (!hasPassword)
            {
                return new Result<Authentication>()
                {
                    Errors = new[] { "Please finish setting up your account first" }
                };
            }

            var loginResult = await _userManager.CheckPasswordAsync(existingUser, userLoginRequest.Password);

            if (!loginResult)
            {
                return new Result<Authentication>()
                {
                    //Todo: be less specific
                    Errors = new[] { "Wrong password" }
                };
            }

            //if (!existingUser.EmailConfirmed)
            //{
            //    return new Result<Authentication>()
            //    {
            //        Errors = new[] { "Please confirm email first" }
            //    };
            //}

            //create jwtToken & refreshToken for successful login 
            return await GenerateAuthResult(existingUser); 
        }


        #region DSA
        public async Task<Result<Authentication>> RefreshTokenAsync(UserRefreshTokenRequest refreshTokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //1) Validation 1 - validate JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(refreshTokenRequest.AccessToken, _tokenValidationParameters, out var validatedToken);


                //2) Validation 2 - validate encryption algorithm, checks if the jwt token has been encrypted using the encryption that we have specified.
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var a = jwtSecurityToken.Header.Alg;
                    var b = SecurityAlgorithms.HmacSha256;
                    var result = a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        return new Result<Authentication>()
                        {
                            Errors = new[] { "Invalid JWT AccessToken" }
                        };
                    }
                }

                //3) Validation 3 - check the expiry time of that token
                var ExpiryDateUnix = long.Parse(tokenInVerification.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var ExpiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(ExpiryDateUnix);//.ToUniversalTime();


                if (ExpiryDateTimeUtc > DateTime.UtcNow)
                {
                    //refresh has not expired
                    return new Result<Authentication>() { Errors = new[] { "JWT AccessToken hasn't expired yet" } };
                }

                //4) Validation 4 - validate existence of the token in the database
                var storedToken = await _context.RefreshTokens.FindAsync(refreshTokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new Result<Authentication>() { Errors = new[] { "This refresh token doesn't exist" } };
                }

                //5) Validation 5 - validate if used or not 
                if (storedToken.isUsed)
                {
                    return new Result<Authentication>() { Errors = new[] { "This refresh token has been used already" } };
                }

                //6) Validation 6 - validate if revoked
                if (storedToken.Invalidated)
                {
                    return new Result<Authentication>() { Errors = new[] { "This refresh token has been invalidated" } };
                }

                //7) Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return new Result<Authentication>() { Errors = new[] { "This refresh token doesn't match this JWT" } };
                }

                // Validation 8 - validate if stored refresh token has expired date (6months)
                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new Result<Authentication>() { Errors = new[] { "This refresh token has expired" } };
                }

                // Update current token
                storedToken.isUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                // Generate a new token
                var currentUser = await _userManager.FindByIdAsync(storedToken.UserId);


                return await GenerateAuthResult(currentUser);
            }
            catch (Exception ex)
            {

                return new Result<Authentication>() { Errors = new[] { ex.Message } };
            }
        }


        public async Task<Result<object>> CompleteRegistration(UserCompleteRegistrationRequest completeRegistrationRequest)
        {

            var user = await _userManager.FindByIdAsync(completeRegistrationRequest.userId);
            if (user == null)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"Failed to complete registration because user '{completeRegistrationRequest.userId}' was not found" }
                };
            }

            //Check if user has confirmed their email first
            //var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            //if (!emailConfirmed)
            //{
            //    return new Result<object>()
            //    {
            //        Errors = new List<string>() { $"Failed to complete registration because user '{completeRegistrationRequest.userId}' has not confirmed their email yet." }
            //    };
            //}

            //Check if user has already completed registration (has a password)
            //var hasPassword = await _userManager.HasPasswordAsync(user);
            //if (hasPassword)
            //{
            //    return new Result<object>()
            //    {
            //        Errors = new List<string>() { $"Failed to complete registration because user '{completeRegistrationRequest.userId}' has already completed registration." }
            //    };
            //}


            user.FirstName = completeRegistrationRequest.FirstName;
            user.LastName = completeRegistrationRequest.LastName;

            await _userManager.AddPasswordAsync(user, completeRegistrationRequest.Password);

            return new Result<object>
            {
                Success = true,
                Data = new { Message = "Completed registration successfuly." }
            };


        }

        public async Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token)
        {

            var user = await this.GetCurrentUserById(userId);
            if (user == null)
            {
                return new Result<object>()
                {
                    Errors = new List<string>() { $"User '{userId}' not found" }
                };
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new Result<object>()
                {
                    Errors = result.Errors.Select(x => x.Description).ToList()
                };
            }
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();


            return new Result<object>
            {
                Success = true,
                Data = new
                {
                    UserId = user.Id,
                    Message = "Email confirmed successfully"
                }
            };


        }
        public async Task<object> RevokeTokensAsync(string userId)
        {
            var tokens = await _context.RefreshTokens.Where(token => token.UserId == userId).ToListAsync();
            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();

            //send back list of deleted refresh tokens to the user
            return new Result<object>
            {
                Success = true,
                Data = new { deleted_tokens = tokens }
            };
        }

        #endregion
        #region HELPER METHODS
        private async Task<ApplicationUser> GetCurrentUserById(string getUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(getUserId);
            if (currentUser == null) return null;

            return currentUser;
        }
        private async Task<List<Claim>> getAllUserClaims(ApplicationUser user)
        {

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim("GarderieId", user.GarderieId == null ? "": user.GarderieId.ToString() ),

                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FirstName", user.FirstName is  not null ? user.FirstName:""),
                new Claim("LastName", user.LastName is not null ? user.LastName:""),

                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //get claims of current user
            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);

            //get roles of the current user
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }

            }
            return claims;
        }

        //Generate AccessToken && RefreshToken
        public async Task<Result<Authentication>> GenerateAuthResult(ApplicationUser user)
        {


            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = await getAllUserClaims(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),//token will expire in X minutes/seconds
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();



            return new Result<Authentication>()
            {
                Success = true,
                Data = new Authentication
                {
                    AccessToken = jwtTokenHandler.WriteToken(token),
                    expiresIn = Convert.ToInt16((token.ValidTo - DateTime.UtcNow).TotalMinutes),
                    RefreshToken = refreshToken.Token
                }
            };
        }
        private DateTime UnixTimeStampToDateTime(long utcExpiryDate)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(utcExpiryDate).ToUniversalTime();
            return dateTimeValue;
        }

   
        #endregion
    }
}
