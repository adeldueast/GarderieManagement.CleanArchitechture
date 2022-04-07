using GarderieManagementClean.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace GarderieManagementClean.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Photo { get; set; }

        public int? GarderieId { get; set; }

        public Garderie Garderie { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    }
}
