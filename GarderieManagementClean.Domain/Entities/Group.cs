using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string HexColor { get; set; }



        //this is the one to one relation with ApplicationUser

        [ForeignKey("ApplicationUser")]
        public string EducatriceId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
