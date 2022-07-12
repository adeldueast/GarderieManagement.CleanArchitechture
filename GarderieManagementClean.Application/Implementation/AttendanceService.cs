using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Implementation
{
    public class AttendanceService : IAttendanceService
    {

        public IAttendanceService _attendanceRepository { get; }
        public AttendanceService(IAttendanceService attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }
        public Task<Result<Attendance>> arrivedAt(string userId, int enfantId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Attendance>> createAbsence(string userId, AttendanceCreateAbsenceRequest attendanceCreateAbsenceRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Attendance>> leftAt(string userId, int enfantId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Attendance>> removeAbsence(string userId, AttendanceDeleteAbsence attendanceDeleteAbsence)
        {
            throw new NotImplementedException();
        }
    }
}
