using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Photo { get; set; }

        public virtual List<Enfant>  Enfants { get; set; }

        public int GarderieId { get; set; }

        public virtual Garderie Garderie { get; set; }
    }
}
