using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class GroupUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Photo { get; set; }
    }
}
