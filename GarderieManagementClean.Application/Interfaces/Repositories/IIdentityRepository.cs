using Contracts.Dtos.Request;
using Contracts.Request;
using GarderieManagementClean.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces
{
    public interface IIdentityRepository
    {

        public Task<Result<Authentication>> RegisterOwnerAsync(UserRegistrationRequest userRegistrationRequest);

        public Task<Result<object>> InviteUser(string userId, string email, string role);

        public Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token);

        public Task<Result<object>> CompleteRegistration(CompleteRegistrationRequest completeRegistrationRequest);

        public Task<Result<Authentication>> AuthenticateAsync(UserLoginRequest userLoginRequest);

        public Task<Result<Authentication>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);





        public Task<object> RevokeTokensAsync(string userId);


    }
}
