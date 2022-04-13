using GarderieManagementClean.Application.Models;
using System;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces
{
    public interface IIdentityService
    {
        //Register
        public Task<Result<Authentication>> RegisterOwnerAsync(string email, string password);


        //Login
        public Task<Result<Authentication>> AuthenticateAsync(string email, string password);


        //Confirm Email OR Confirm Invitation
        public Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token);

    


        public Task<Result<object>> InviteUser(string email, string role);

        //REFRESH TOKENS
        public Task<object> RevokeTokensAsync(string userId);
        public Task<Result<Authentication>> RefreshTokenAsync(string Token, string RefreshToken);


    }
}
