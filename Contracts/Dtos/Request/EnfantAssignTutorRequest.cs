using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class EnfantAssignTutorRequest
    {
        public string TutorId { get; set; }

        public int EnfantId { get; set; }

        public string Relation { get; set; }

        public bool EmergencyContact { get; set; }
        public bool AuthorizePickup { get; set; }


    }
}
