using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GarderieManagementClean.Infrastructure.Identity
{
    public class RefreshToken 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool isUsed { get; set; } = false;

        public bool Invalidated { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
