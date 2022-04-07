using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos
{
    public class AddressDTO
    {
        public string Ville { get; set; }

        public string Rue { get; set; }

        public string Province { get; set; }

        public string CodePostal { get; set; }

        public string Telephone { get; set; }

    }
}
