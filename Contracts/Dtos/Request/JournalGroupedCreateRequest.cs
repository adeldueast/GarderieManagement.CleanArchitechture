using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class JournalGroupedCreateRequest
    {

        public IEnumerable<RatingDTO> Ratings { get; set; }
        public string Activite_Message { get; set; }
        public string Manger_Message { get; set; }

    }
}
