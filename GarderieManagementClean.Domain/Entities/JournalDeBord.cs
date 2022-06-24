using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class JournalDeBord
    {
        public int Id { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Humeur_Rating { get; set; }
        public string Humeur_Description { get; set; }


        public int Toilette_Rating { get; set; }
        public string Toilette_Description { get; set; }


        public int Manger_Rating { get; set; }
        public string Manger_Description { get; set; }


        public string Message { get; set; }


        public int EnfantId { get; set; }
        public virtual Enfant Enfant { get; set; }

        //this is the one to one relation with ApplicationUser
        [ForeignKey("ApplicationUser")]
        public string EducatriceId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public string LastUpdatedBy { get; set; }








    }
}
