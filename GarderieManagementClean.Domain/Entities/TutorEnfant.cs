using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class TutorEnfant
    {
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


        public int EnfantId { get; set; }
        public virtual Enfant Enfant { get; set; }


        public string Relation { get; set; }
    }
}
