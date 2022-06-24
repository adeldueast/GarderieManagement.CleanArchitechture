using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public  class Local
    {
        public int Id { get; set; }


        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public virtual List<Enfant> Enfants { get; set; }
    }
}
