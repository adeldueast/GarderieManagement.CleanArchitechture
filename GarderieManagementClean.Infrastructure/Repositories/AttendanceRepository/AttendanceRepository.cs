using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Infrastructure.Repositories.AttendanceRepository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Result<Attendance>> arrivedAt(string userId, int enfantId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            //Check if child exist
            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant is null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Enfant '{enfantId}' doesnt exist " }
                };
            }

            //Check if any attendance was made today 
            var attendance = await _context.Attendances.SingleOrDefaultAsync(e =>
                 e.AbsenceDate == DateTime.Now &&
                 e.EnfantId == enfantId
                );

            //An attendance already exist (could just be an Absence tho)
            if (attendance != null)
            {
                if (attendance.ArrivedAt != null)
                {
                    return new Result<Attendance>()
                    {
                        Errors = new List<string>() { $"Enfant '{enfantId}' has already arrived" }
                    };
                }

                attendance.ArrivedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return new Result<Attendance>()
                {
                    Success = true,
                    Data = attendance
                };
            }

            attendance = new Attendance()
            {
                Enfant = enfant,
                ArrivedAt = DateTime.Now,
                AbsenceDate = DateTime.Now,
            };

            _context.Add(attendance);
            await _context.SaveChangesAsync();

            return new Result<Attendance>()
            {
                Success = true,
                Data = attendance
            };


        }

        public async Task<Result<Attendance>> leftAt(string userId, int enfantId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            //Check if child exist
            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant is null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Enfant '{enfantId}' doesnt exist " }
                };
            }

            //Check if any attendance was made today 
            var attendance = _context.Attendances.FirstOrDefault(e =>
                e.AbsenceDate == DateTime.Now &&
                e.EnfantId == enfantId
                );


            if (attendance == null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Enfant '{enfantId}' has not arrived yet" }
                };

            }


            if (attendance.ArrivedAt == null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Enfant '{enfantId}' has not arrived yet" }
                };
            }

            //TODO: Check if LeftAt.Date - ArrivedAt.Date  > X hours. If not it simply could bea missclick from the user

            attendance.LeftAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return new Result<Attendance>()
            {
                Success = true,
                Data = attendance
            };



        }

        public async Task<Result<Attendance>> createAbsence(string userId, AttendanceCreateAbsenceRequest attendanceCreateAbsenceRequest)
        {
            var user = await _userManager.FindByIdAsync(userId);

            //Check if child exist
            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.Id == attendanceCreateAbsenceRequest.EnfantId && e.GarderieId == user.GarderieId);
            if (enfant is null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Enfant '{attendanceCreateAbsenceRequest.EnfantId}' doesnt exist " }
                };
            }

            var attendance = _context.Attendances.SingleOrDefault(e =>
               e.AbsenceDate == attendanceCreateAbsenceRequest.AbsenceDate &&
               e.EnfantId == attendanceCreateAbsenceRequest.EnfantId
               );

            if (attendance != null)
            {
                if (attendance.ArrivedAt != null)
                {
                    return new Result<Attendance>()
                    {
                        Errors = new List<string>() { $"Enfant '{attendanceCreateAbsenceRequest.EnfantId}' cant be absent because he has attended" }
                    };
                }

                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Enfant '{attendanceCreateAbsenceRequest.EnfantId}' is already set absent for today, instead update the absence." }
                };
            }

            attendance = new Attendance()
            {
                Enfant = enfant,
                AbsenceDescription = attendanceCreateAbsenceRequest.AbsenceDescription,
                AbsenceDate = attendanceCreateAbsenceRequest.AbsenceDate,
            };

            return new Result<Attendance>()
            {
                Success = true,
                Data = attendance
            };
        }



        public async Task<Result<Attendance>> removeAbsence(string userId, AttendanceDeleteAbsence attendanceDeleteAbsence)
        {
            var user = await _userManager.FindByIdAsync(userId);

            //Check if child exist
            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.Id == attendanceDeleteAbsence.EnfantId && e.GarderieId == user.GarderieId);
            if (enfant is null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"because enfant '{attendanceDeleteAbsence.EnfantId}' doesnt exist " }
                };
            }

            var attendance = _context.Attendances.SingleOrDefaultAsync(e =>
               e.Id == attendanceDeleteAbsence.AttendanceId &&
               e.EnfantId == attendanceDeleteAbsence.EnfantId
               );

            if (attendance == null)
            {
                return new Result<Attendance>()
                {
                    Errors = new List<string>() { $"Failed to remove absence '{attendanceDeleteAbsence.AttendanceId}' because it doesnt exist" }
                };
            }



            _context.Remove(attendance);
            await _context.SaveChangesAsync();


            return new Result<Attendance>()
            {
                Success = true,
                Data = attendance
            };
        }
    }
}
