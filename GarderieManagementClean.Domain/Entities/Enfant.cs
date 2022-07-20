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
        public string Photo { get; set; } = "https://cdn.vox-cdn.com/thumbor/23dWY86RxkdF7ZegvfnY8gFjR7s=/1400x1400/filters:format(jpeg)/cdn.vox-cdn.com/uploads/chorus_asset/file/19157811/ply0947_fall_reviews_2019_tv_anime.jpg";


        public int GarderieId { get; set; }
        public virtual Garderie Garderie { get; set; }


        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }


        public virtual List<TutorEnfant> Tutors { get; set; } = new List<TutorEnfant>();


        public virtual List<Attendance> Attendances { get; set; }


        public int? LocalId { get; set; }
        public virtual Local Local { get; set; }
    }
}
