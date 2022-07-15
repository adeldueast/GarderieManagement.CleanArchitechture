using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class JournalUpdateRequest
    {



        public int Humeur_Rating { get; set; }
        public int Toilette_Rating { get; set; }
        public int Manger_Rating { get; set; }
        public int Participation_Rating { get; set; }



        public string Activite_Message { get; set; }
        public string Manger_Message { get; set; }
        public string Commentaire_Message { get; set; }






    }
}
