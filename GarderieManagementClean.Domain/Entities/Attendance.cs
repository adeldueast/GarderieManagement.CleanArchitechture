﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Domain.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        public DateTime? ArrivedAt { get; set; }
        public DateTime? LeftAt { get; set; }

        public DateTime? AbsenceDate { get; set; } 
        public string AbsenceDescription { get; set; }


        public int EnfantId { get; set; }
        public virtual Enfant Enfant { get; set; }

    }
}
