using System;
using System.Collections.Generic;
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



   

        public virtual List<Enfant> Enfants { get; set; }



        public int? EnfantId { get; set; }
        public virtual Enfant? PhotoCouvertureDe { get; set; }


        //public bool isPhotoCouverture { get => this.PhotoCouvertureDe != null; }

    }
}
