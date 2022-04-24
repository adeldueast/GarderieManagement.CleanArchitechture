using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = GarderieManagementClean.Domain.Entities.Group;

namespace Contracts.Dtos.Response
{
    public class EnfantResponse
    {

        public int Id { get; set; }

        public string Nom { get; set; }

        public DateTime DateNaissance { get; set; }

        public string Photo { get; set; } = "www.photo-url.com";

        public GroupDTO Group { get; set; }

      
    }
}
