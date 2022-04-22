using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class UserInviteUserRequest
    {
        public string Email { get; set; }
        public string role { get; set; }
    }
}
