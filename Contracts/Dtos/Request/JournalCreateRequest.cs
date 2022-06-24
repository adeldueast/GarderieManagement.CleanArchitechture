using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class JournalCreateRequest
    {
   
        public int Humeur_Rating { get; set; }
        public string Humeur_Description { get; set; }


        public int Toilette_Rating { get; set; }
        public string Toilette_Description { get; set; }


        public int Manger_Rating { get; set; }
        public string Manger_Description { get; set; }


        public string Message { get; set; }


        public int EnfantId { get; set; }
   

 
    }
}
