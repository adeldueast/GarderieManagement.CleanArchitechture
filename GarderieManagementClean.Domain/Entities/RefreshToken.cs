using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
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
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
