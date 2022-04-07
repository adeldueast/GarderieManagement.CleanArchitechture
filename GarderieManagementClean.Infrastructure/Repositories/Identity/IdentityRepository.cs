using GarderieManagementClean.Application.Interfaces;
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



namespace GarderieManagementClean.Infrastructure.Identity
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityRepository(

             UserManager<ApplicationUser> userManager,
             RoleManager<IdentityRole> roleManager,
             JwtSettings jwtSettings,
             ApplicationDbContext context, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            // _logger = logger;
        }
        public async Task<Result<Authentication>> RegisterOwnerAsync(string email, string password)
        {


            var newUser = new ApplicationUser
            {
                Email = email,
                UserName = email,
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
            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {

                return new Result<Authentication>
                {
                    Errors = createdUser.Errors.Select(err => err.Description)
                };
            };

            //Assign role to newUser
            await _userManager.AddToRoleAsync(newUser, "owner");



            //create a token for successful registration 
            return await GenerateAuthResult(newUser);

        }


        public async Task<Result<Authentication>> LoginAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                return new Result<Authentication>()
                {
                    //Todo: be less specific
                    Errors = new[] { "Wrong email" }
                };
            }

            var loginResult = await _userManager.CheckPasswordAsync(existingUser, password);

            if (!loginResult)
            {
                return new Result<Authentication>()
                {
                    //Todo: be less specific
                    Errors = new[] { "Wrong password" }
                };
            }

            //create jwtToken & refreshToken for successful login 
            return await GenerateAuthResult(existingUser); ;
        }


        public async Task<Result<Authentication>> RefreshTokenAsync(string Token, string RefreshToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //1) Validation 1 - validate JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(Token, _tokenValidationParameters, out var validatedToken);


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
                var storedToken = await _context.RefreshTokens.FindAsync(RefreshToken);

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


        public async Task<object> RevokeTokensAsync(string userId)
        {
            var tokens = await _context.RefreshTokens.Where(token => token.UserId == userId).ToListAsync();
            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();

            //send back list of deleted refresh tokens to the user
            return new Result<object>
            {
                Success = true,
                Data = new { tokens_deleted = tokens }
            };
        }

        //Helper methods
        public async Task<ApplicationUser> GetCurrentUser(Func<string> getUserId)
        {
            var currentUser = await _userManager.FindByIdAsync(getUserId());
            if (currentUser == null) return null;

            return currentUser;
        }
        private async Task<List<Claim>> getAllUserClaims(ApplicationUser user)
        {

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
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

        private async Task<Result<Authentication>> GenerateAuthResult(ApplicationUser user)
        {


            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = await getAllUserClaims(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(30),//token will expire in X minutes/seconds
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
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new Result<Authentication>()
            {
                Success = true,
                Data = new Authentication
                {
                    AccessToken = jwtTokenHandler.WriteToken(token),
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
    }
}
