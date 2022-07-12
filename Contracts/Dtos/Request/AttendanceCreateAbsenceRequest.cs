using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class AttendanceCreateAbsenceRequest
    {
        public int EnfantId { get; set; }

        public DateTime AbsenceDate { get; set; }

        public string AbsenceDescription { get; set; }

    }
}
