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
        public async Task<Result<Authentication>> AuthenticateAsync(string email, string password)
        {
            var AuthResult = await _identityRepository.AuthenticateAsync(email, password);
            return AuthResult;
        }

        public async Task<Result<object>> ConfirmEmailOrInvitationAsync(string userId, string token)
        {
            var result = await _identityRepository.ConfirmEmailOrInvitationAsync(userId, token);
            return result;
        }
        public async Task<Result<Authentication>> RefreshTokenAsync(string Token, string RefreshToken)
        {
            var AuthResult = await _identityRepository.RefreshTokenAsync(Token, RefreshToken);
            return AuthResult;
        }

        public async Task<Result<Authentication>> RegisterOwnerAsync(string email, string password)
        {
            var AuthResult = await _identityRepository.RegisterOwnerAsync(email, password);
            return AuthResult;
        }

        public async Task<object> RevokeTokensAsync(string userId)
        {
            var Result = await _identityRepository.RevokeTokensAsync(userId);
            return Result;
        }

        public async Task<Result<object>> InviteUser(string email, string role)
        {
            var result = await _identityRepository.InviteUser(email, role);
            return result;
        }

    }
}
