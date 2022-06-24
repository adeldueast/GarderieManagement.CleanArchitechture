using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GarderieManagementClean.API.Controllers.V1
{
    [Authorize(Roles = "owner,admin,employee")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AttendanciesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        [HttpPost("Arrived/{enfantId}")]
        public async Task<IActionResult> Arrived([FromRoute] int enfantId)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var enfant = _context.Enfants.SingleOrDefault(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant == null) return NotFound($"Enfant '{enfantId}' does not exist");

            //Check if an attendance record already exist
            var attendance = _context.Attendances.SingleOrDefault(attendance => attendance.EnfantId == enfant.Id && attendance.Date.Date == DateTime.Now.Date);

            //If Attendance already exist
            //1- He already arrived
            //2- He was placed absent for that current day
            if (attendance != null)
            {
                attendance.ArrivedAt = DateTime.Now;
                attendance.LeftAt = null;
                attendance.AbsenceDescription = "";
                await _context.SaveChangesAsync();
                return Ok(attendance);
            }


            //Else, simply create a new attendance with a date (present)
            attendance = new Attendance
            {
                Enfant = enfant,
                ArrivedAt = DateTime.Now,
            };


            _context.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(attendance);



        }

        [HttpPost("Left/{enfantId}")]
        public async Task<IActionResult> Left([FromRoute] int enfantId)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var enfant = _context.Enfants.SingleOrDefault(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant == null) return NotFound($"Enfant '{enfantId}' does not exist");

            var attendance = _context.Attendances.SingleOrDefault(attendance => attendance.EnfantId == enfant.Id && attendance.Date.Date == DateTime.Now.Date);
            
            //If Attendance doesnt exist
            if (attendance == null) return BadRequest($"Enfant '{enfantId}' has not arrived yet");

            if (attendance.ArrivedAt == null) return BadRequest($"Enfant '{enfantId}' has not arrived yet");


            attendance.LeftAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(attendance);



        }


        [HttpPost("Absent/{enfantId}")]
        public async Task<IActionResult> Absent([FromRoute] int enfantId, [FromBody] string AbsenceReason)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);
            var enfant = _context.Enfants.SingleOrDefault(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant == null) return NotFound($"Enfant '{enfantId}' does not exist");

            var attendance = _context.Attendances.SingleOrDefault(attendance => attendance.EnfantId == enfant.Id && attendance.Date.Date == DateTime.Now.Date);

            //If attendance exists
            //1) he was present 
            //2) he was absent
            //in both case, we simply clear all and give absence reason 
            if (attendance != null)
            {
                attendance.ArrivedAt = null;
                attendance.LeftAt = null;
                attendance.AbsenceDescription = AbsenceReason;
                await _context.SaveChangesAsync();
                return Ok(attendance);
            }

            //Else, create absence attendance (ArrivedAt = null)
            attendance = new Attendance
            {
                Id = enfantId,
                AbsenceDescription = AbsenceReason,

            };
            _context.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(attendance);

        }


    }
}
