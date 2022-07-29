using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class Photo
    {

        public int Id { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }


        public string Description { get; set; }



        //A photo either has a list of children because it is of type gallerie
        public virtual List<Enfant> Enfants { get; set; }


        //or it has one child relation because it is a profile picture

        public int? EnfantId { get; set; }
        public virtual Enfant PhotoCouvertureDe { get; set; }


     

    }
}
