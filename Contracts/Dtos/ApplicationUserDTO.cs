using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public string Photo { get; set; }


        public string Email { get; set; }


        public bool EmergencyContact { get; set; }
        public bool AuthorizePickup { get; set; }

        public bool isOnline { get; set; }

        //TODO: Add Phone and Address and isAuthorize to pickup the child and shouldContact in emergency case 

    }
}
