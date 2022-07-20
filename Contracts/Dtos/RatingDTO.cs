using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int Humeur_Rating { get; set; }
        public int Toilette_Rating { get; set; }
        public int Manger_Rating { get; set; }
        public int Participation_Rating { get; set; }

    }
}
