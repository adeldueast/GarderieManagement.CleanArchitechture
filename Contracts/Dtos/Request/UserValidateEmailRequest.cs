using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class UserValidateEmailRequest
    {
        public string userId { get; set; }
        public string ConfirmEmailToken { get; set; }
    }
}
