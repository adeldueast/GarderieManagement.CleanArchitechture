using Contracts.Dtos.Request;
using Contracts.Request;
using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.ServicesImplementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;

        //Constructor Dependency Injector
        public IdentityService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<Result<Authentication>> AuthenticateAsync(UserLoginRequest userLoginRequest)
        {
            var AuthResult = await _identityRepository.AuthenticateAsync(userLoginRequest);
            return AuthResult;
        }

        public async Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId,string token)
        {
            var result = await _identityRepository.ConfirmEmailOrInvitationAsync(userId, token);
            return result;
        }

        public async Task<Result<Authentication>> RefreshTokenAsync(UserRefreshTokenRequest refreshTokenRequest)
        {
            var AuthResult = await _identityRepository.RefreshTokenAsync(refreshTokenRequest);
            return AuthResult;
        }

        public async Task<Result<Authentication>> RegisterOwnerAsync(UserRegistrationRequest userRegistrationRequest)
        {
            var AuthResult = await _identityRepository.RegisterOwnerAsync(userRegistrationRequest);
            return AuthResult;
        }

        public async Task<object> RevokeTokensAsync(string userId)
        {
            var Result = await _identityRepository.RevokeTokensAsync(userId);
            return Result;
        }

        public async Task<Result<object>> InviteUser(string userId, UserInviteUserRequest inviteUserRequest)
        {
            var result = await _identityRepository.InviteUser( userId,  inviteUserRequest);
            return result;
        }
        public async Task<Result<object>> InviteTutor(string userId, UserInviteTutorRequest inviteUserRequest)
        {
            var result = await _identityRepository.InviteTutor(userId, inviteUserRequest);
            return result;
        }

      

        public async Task<Result<object>> CompleteRegistration( UserCompleteRegistrationRequest completeRegistrationRequest)
        {
            var result = await _identityRepository.CompleteRegistration(completeRegistrationRequest);
            return result;
        }
    }
}
