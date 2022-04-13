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

        public Task<Result<Authentication>> RegisterOwnerAsync(string email, string password);

        public Task<Result<Authentication>> AuthenticateAsync(string email, string password);

        public Task<Result<Authentication>> RefreshTokenAsync(string Token, string RefreshToken);

        public Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token);

        public Task<Result<object>> InviteUser(string email, string role);

        public Task<object> RevokeTokensAsync(string userId);


    }
}
