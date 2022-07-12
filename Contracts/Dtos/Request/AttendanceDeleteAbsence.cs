using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dtos.Request
{
    public class AttendanceDeleteAbsence
    {
        public int EnfantId { get; set; }

        public int AttendanceId { get; set; }

    }
}
