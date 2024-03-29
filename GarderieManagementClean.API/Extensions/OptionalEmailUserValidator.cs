﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API.Extensions
{
    public class OptionalEmailUserValidator<TUser> : UserValidator<TUser> where TUser : class
    {
        public OptionalEmailUserValidator(IdentityErrorDescriber errors = null) : base(errors)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var result = await base.ValidateAsync(manager, user);

            if (!result.Succeeded && (String.IsNullOrWhiteSpace(await manager.GetEmailAsync(user)) || String.IsNullOrWhiteSpace(await manager.GetUserNameAsync(user))))
            {
                var errors = result.Errors.Where(e => e.Code != "InvalidEmail" && e.Code != "InvalidUserName");

                result = errors.Count() > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
            }

            return result;
        }
    }
}
