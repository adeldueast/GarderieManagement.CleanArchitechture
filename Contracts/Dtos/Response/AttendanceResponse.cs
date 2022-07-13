using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Response
{
    public class AttendanceResponse
    {
        public int Id { get; set; }

        public bool Present { get; set; }

        public DateTime Date { get; set; }

        public string AbsenceDescription { get; set; }

    }
}
