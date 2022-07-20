using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos
{
    public class TutorEnfantDTO
    {


        public ApplicationUserDTO ApplicationUser { get; set; }



        public string Relation { get; set; }

        public bool EmergencyContact { get; set; }
        public bool AuthorizePickup { get; set; }
    }
}
