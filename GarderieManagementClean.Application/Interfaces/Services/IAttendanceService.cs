using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Services
{
    public interface IAttendanceService
    {
        public Task<Result<Attendance>> createAbsence(string userId, AttendanceCreateAbsenceRequest attendanceCreateAbsenceRequest);
        public Task<Result<Attendance>> removeAbsence(string userId, AttendanceDeleteAbsence attendanceDeleteAbsence);

        public Task<Result<Attendance>> arrivedAt(string userId, int enfantId);
        public Task<Result<Attendance>> leftAt(string userId, int enfantId);


    }
}
