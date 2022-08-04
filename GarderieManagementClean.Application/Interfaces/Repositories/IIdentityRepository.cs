using Contracts.Dtos.Request;
using Contracts.Request;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
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

        public Task<Result<object>> InviteUser(string userId, UserInviteUserRequest inviteUserRequest);

        public Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token);

        public Task<Result<object>> CompleteRegistration(UserCompleteRegistrationRequest completeRegistrationRequest);

        public Task<Result<Authentication>> AuthenticateAsync(UserLoginRequest userLoginRequest);

        public Task<Result<Authentication>> RefreshTokenAsync(UserRefreshTokenRequest refreshTokenRequest);

        public Task<Result<object>> InviteTutor(string userId, UserInviteTutorRequest inviteTutorRequest);


        public Task<Result<Authentication>> GenerateAuthResult(ApplicationUser user);

        public Task<object> RevokeTokensAsync(string userId);


    }
}
