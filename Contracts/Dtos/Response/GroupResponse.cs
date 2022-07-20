using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Response
{
    public class GroupResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public string Photo { get; set; }

        public string EducatriceFullName { get; set; }

        public List<dynamic> Enfants { get; set; }

        public string HexColor { get; set; }

    }
}
