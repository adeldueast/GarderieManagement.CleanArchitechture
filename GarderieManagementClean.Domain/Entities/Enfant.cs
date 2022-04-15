using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class Enfant
    {

        public int Id { get; set; }

        public string Nom { get; set; }

        public DateTime DateNaissance { get; set; }

        public string Photo { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

    }
}
