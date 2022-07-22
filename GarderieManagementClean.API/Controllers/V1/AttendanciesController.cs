using AutoMapper;
using Contracts.Dtos.Request;
using Contracts.Dtos.Response;
using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.API.HubConfig;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GarderieManagementClean.API.Controllers.V1
{
    [Authorize(Roles = "owner,admin,employee")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AttendanciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChildrenHub> _hubContext;


        public AttendanciesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IHubContext<ChildrenHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _hubContext = hubContext;
        }



        [HttpPost("Arrived/{enfantId}")]
        public async Task<IActionResult> Arrived([FromRoute] int enfantId)
        {
            //Get current loggedIn User
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            //Get Child based on current User's organization (GarderieId => GurserieId ) and ChildId
            var enfant = _context.Enfants.AsNoTracking().SingleOrDefault(e =>
                e.Id == enfantId &&
                e.GarderieId == user.GarderieId
            );

            //Child not found error response
            if (enfant == null) return NotFound($"Enfant '{enfantId}' does not exist");

            //Check if an attendance record already exist for today
            var attendance = await _context.Attendances.SingleOrDefaultAsync(attendance =>
            attendance.EnfantId == enfant.Id &&
            (

            attendance.ArrivedAt.Value.Date == DateTime.Now.Date ||
            attendance.AbsenceDate.Value.Date == DateTime.Now.Date
            )

            );

            //If Attendance already exist
            //1- He either arrived in the nurserie
            //2- He was set as  Absent for that current day (ArrivedAt = null BUT AbsenceDate is not null ) 
            //in both case we clear the absence, and simply set the arrivedAt to Current DateTime
            if (attendance != null)
            {

             


                attendance.ArrivedAt = DateTime.Now;
                attendance.LeftAt = null;
                attendance.AbsenceDate = null;
                attendance.AbsenceDescription = null;
                await _context.SaveChangesAsync();
                var dto_attendance1 = _mapper.Map<AttendanceResponse>(attendance);

                Request.Headers.TryGetValue("x-signalr-connection", out var signalRConnectionId2);
                await _hubContext.Clients.GroupExcept(HttpContext.GetUserGarderieId(), signalRConnectionId2).SendAsync("childAttendanceUpdate", dto_attendance1);


                return Ok(dto_attendance1);

            }


            //Else, simply create a new attendance with a date (present)
            attendance = new Attendance
            {
                EnfantId = enfant.Id,
                ArrivedAt = DateTime.Now,
            };



            _context.Add(attendance);
            await _context.SaveChangesAsync();

            var dto_attendance = _mapper.Map<AttendanceResponse>(attendance);
            //NOTIFY ALL SUBSCRIBED CLIENTS 
            Request.Headers.TryGetValue("x-signalr-connection", out var signalRConnectionId);
            await _hubContext.Clients.GroupExcept(HttpContext.GetUserGarderieId(), signalRConnectionId).SendAsync("childAttendanceUpdate", dto_attendance);

            return Ok(dto_attendance);



        }

        [HttpPost("Left/{enfantId}")]
        public async Task<IActionResult> Left([FromRoute] int enfantId)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var enfant = _context.Enfants.AsNoTracking().SingleOrDefault(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant == null) return NotFound($"Enfant '{enfantId}' does not exist");

            var attendance = await _context.Attendances.SingleOrDefaultAsync(attendance =>
            attendance.EnfantId == enfant.Id &&
            attendance.ArrivedAt.Value.Date == DateTime.Now.Date
            );

            //If Attendance doesnt exist
            if (attendance == null) return BadRequest($"Enfant '{enfantId}' has not arrived yet");



            attendance.LeftAt = DateTime.Now;
            await _context.SaveChangesAsync();
            var dto_attendance = _mapper.Map<AttendanceResponse>(attendance);
            Request.Headers.TryGetValue("x-signalr-connection", out var signalRConnectionId);
            await _hubContext.Clients.GroupExcept(HttpContext.GetUserGarderieId(), signalRConnectionId).SendAsync("childAttendanceUpdate", dto_attendance);
            return Ok(dto_attendance);




        }


        [HttpPost("Absent/{enfantId}")]
        public async Task<IActionResult> createAbsence([FromBody] AttendanceCreateAbsenceRequest attendanceCreateAbsenceRequest)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var enfant = _context.Enfants.AsNoTracking().SingleOrDefault(e => e.Id == attendanceCreateAbsenceRequest.EnfantId && e.GarderieId == user.GarderieId);
            if (enfant == null) return NotFound($"Enfant '{attendanceCreateAbsenceRequest.EnfantId}' does not exist");

            var attendance = await _context.Attendances.SingleOrDefaultAsync(attendance =>
                attendance.EnfantId == enfant.Id &&
                ( attendance.AbsenceDate.Value.Date == attendanceCreateAbsenceRequest.AbsenceDate.Date ||attendance.ArrivedAt.Value.Date == attendanceCreateAbsenceRequest.AbsenceDate.Date)
                );

            //If attendance exists
            //1) he has arrived (present) 
            //2) he was set to Absent for that day
            //in both case, we simply clear all and give absence reason 
            if (attendance != null)
            {
                if (attendance.AbsenceDate.HasValue)
                {
                    return BadRequest();
                }

                attendance.ArrivedAt = null;
                attendance.LeftAt = null;
                attendance.AbsenceDate = attendanceCreateAbsenceRequest.AbsenceDate;
                attendance.AbsenceDescription = attendanceCreateAbsenceRequest.AbsenceDescription;
                await _context.SaveChangesAsync();
                var dto_attendance1 = _mapper.Map<AttendanceResponse>(attendance);
                Request.Headers.TryGetValue("x-signalr-connection", out var signalRConnectionId2);
                await _hubContext.Clients.GroupExcept(HttpContext.GetUserGarderieId(), signalRConnectionId2).SendAsync("childAttendanceUpdate", dto_attendance1);
                return Ok(dto_attendance1);
            }

            //Else, create absence attendance (ArrivedAt = null)
            attendance = new Attendance
            {
                EnfantId = attendanceCreateAbsenceRequest.EnfantId,
                AbsenceDate = attendanceCreateAbsenceRequest.AbsenceDate,
                AbsenceDescription = attendanceCreateAbsenceRequest.AbsenceDescription,

            };

            _context.Add(attendance);
            await _context.SaveChangesAsync();
            var dto_attendance = _mapper.Map<AttendanceResponse>(attendance);
            Request.Headers.TryGetValue("x-signalr-connection", out var signalRConnectionId);
            await _hubContext.Clients.GroupExcept(HttpContext.GetUserGarderieId(), signalRConnectionId).SendAsync("childAttendanceUpdate", dto_attendance);
            return Ok(dto_attendance);

        }


        [HttpGet("{enfantId}")]
        public async Task<IActionResult> getChildsAttendances([FromRoute] int enfantId)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var enfant = _context.Enfants.AsNoTracking().SingleOrDefault(e =>
            e.Id == enfantId &&
            e.GarderieId == user.GarderieId
            );
            if (enfant == null) return NotFound($"Enfant '{enfantId}' does not exist");


            var enfant_attendances = await _context.Attendances
                .Where(a => a.EnfantId == enfantId)
                .Select(a => new AttendanceResponse()
                {
                    Id = a.Id,
                    Present = a.ArrivedAt.HasValue ? true : false,
                    Date = a.ArrivedAt.HasValue ? a.ArrivedAt.Value : a.AbsenceDate.Value,
                    AbsenceDescription = a.AbsenceDescription,

                })
                .ToListAsync();


            return Ok(enfant_attendances);

        }



    }
}
