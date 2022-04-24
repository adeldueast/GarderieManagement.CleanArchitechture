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
        public string Photo { get; set; } = "www.photo-url.com";


        public int GarderieId { get; set; }
        public virtual Garderie Garderie { get; set; }


        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }


        public virtual List<TutorEnfant> Tutors { get; set; } = new List<TutorEnfant>();


    }
}
