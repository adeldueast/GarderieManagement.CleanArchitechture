using Contracts.Dtos;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Response
{
    public class GarderieResponse
    {
        public string Name { get; set; }

        public AddressDTO Address { get; set; }
    }
}
