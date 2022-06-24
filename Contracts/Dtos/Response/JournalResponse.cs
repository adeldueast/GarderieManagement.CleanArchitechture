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

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Humeur_Rating { get; set; }
        public string Humeur_Description { get; set; }


        public int Toilette_Rating { get; set; }
        public string Toilette_Description { get; set; }


        public int Manger_Rating { get; set; }
        public string Manger_Description { get; set; }


        public string Message { get; set; }


        public int EnfantId { get; set; }


        public string CreatedBy { get; set; }


        public DateTime LastUpdatedAt { get; set; }

        public string LastUpdatedBy { get; set; }


    }
}
