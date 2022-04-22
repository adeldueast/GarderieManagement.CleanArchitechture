﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Photo { get; set; }

        public int? GarderieId { get; set; }

        public Garderie Garderie { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public List<Enfant> Enfants { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
    }
}