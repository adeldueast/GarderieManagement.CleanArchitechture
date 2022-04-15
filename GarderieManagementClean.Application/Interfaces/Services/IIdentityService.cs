using Contracts.Dtos.Request;
using Contracts.Request;
using GarderieManagementClean.Application.Models;
using System;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces
{
    public interface IIdentityService
    {
        //Register
        public Task<Result<Authentication>> RegisterOwnerAsync(UserRegistrationRequest userRegistrationRequest);


        //Login
        public Task<Result<Authentication>> AuthenticateAsync(UserLoginRequest userLoginRequest);


        //Confirm Email OR Confirm Invitation
        public Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token);


        public Task<Result<object>> CompleteRegistration( CompleteRegistrationRequest completeRegistrationRequest);


        public Task<Result<object>> InviteUser(string userId,string email, string role);

        //REFRESH TOKENS
        public Task<object> RevokeTokensAsync(string userId);

        public Task<Result<Authentication>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);


    }
}
