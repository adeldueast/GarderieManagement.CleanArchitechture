using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Response
{
    public class JournalResponse
    {

        public int Id { get; set; }


        public int Humeur_Rating { get; set; }
        public int Toilette_Rating { get; set; }
        public int Manger_Rating { get; set; }
        public int Participation_Rating { get; set; }



        public string Activite_Message { get; set; }
        public string Manger_Message { get; set; }
        public string Commentaire_Message { get; set; }



        public int EnfantId { get; set; }


        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdatedAt { get; set; }

        public string LastUpdatedBy { get; set; }


    }
}
