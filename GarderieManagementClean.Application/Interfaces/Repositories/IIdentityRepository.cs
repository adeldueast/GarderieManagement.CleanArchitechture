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

        public Task<Result<Authentication>> LoginAsync(string email, string password);

        public Task<Result<Authentication>> RefreshTokenAsync(string Token, string RefreshToken);

        public Task<object> RevokeTokensAsync(string userId);
    }
}
