using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Models
{
    public class Authentication
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
