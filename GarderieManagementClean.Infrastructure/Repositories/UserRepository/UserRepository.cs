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

namespace GarderieManagementClean.Infrastructure.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<Result<ApplicationUser>> getAllEmployee(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user.GarderieId is null)
            {
                return new Result<ApplicationUser>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }


            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Name == "employee");
            var employeeUsers = await _context.Users.Where(x => x.GarderieId == user.GarderieId && x.UserRoles.Any(r => r.RoleId == role.Id)).ToListAsync();

            return new Result<ApplicationUser>
            {
                Success = true,
                Data = employeeUsers
            };
        }

        //returns all tutors in the garderie
        public async Task<Result<ApplicationUser>> getAllTutors(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user.GarderieId is null)
            {
                return new Result<ApplicationUser>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }

                };
            }


            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Name == "tutor");
            if (role == null)
            {
                return new Result<ApplicationUser>()
                {

                    Errors = new List<string>() { $"Role '{role}' doesnt exist!" }
                };
            }

            var tutorsUsers = await _context.Users
                .Where(
                x => x.GarderieId == user.GarderieId &&
                x.UserRoles.Any(r => r.RoleId == role.Id)).ToListAsync();


            return new Result<ApplicationUser>
            {
                Success = true,
                Data = tutorsUsers
            };
        }


        //returns all child's tutor in the garderie
        public async Task<Result<ApplicationUser>> getAllChildsTutors(string userId, int enfantId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user.GarderieId is null)
            {
                return new Result<ApplicationUser>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }

                };
            }

            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.GarderieId == user.GarderieId && e.Id == enfantId);

            if (enfant is null)
            {
                return new Result<ApplicationUser>
                {
                    Errors = new List<string>() { $"Enfant '{enfantId}' does not exist" }

                };
            }


            //var childsTutors = await _context.Users
            //    .Where(
            //    tutor => tutor.GarderieId == user.GarderieId &&
            //    tutor.Tutors.Any(r => r.EnfantId == enfantId)).ToListAsync();

            var ct = await _context.TutorEnfant.Where(ct => ct.EnfantId == enfantId).ToListAsync();

            return new Result<ApplicationUser>
            {
                Success = true,
                Data = ct
            };
        }

        public async Task<Result<ApplicationUser>> getAllEmployeeWithNoGroup(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user.GarderieId is null)
            {
                return new Result<ApplicationUser>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }


            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Name == "employee");
            var employeeUsers = await _context.Users.Where(x => 
            x.GarderieId == user.GarderieId && 
            x.Group == null &&
            x.UserRoles.Any(r => r.RoleId == role.Id)).ToListAsync();

            return new Result<ApplicationUser>
            {
                Success = true,
                Data = employeeUsers
            };
        }
    }
}
