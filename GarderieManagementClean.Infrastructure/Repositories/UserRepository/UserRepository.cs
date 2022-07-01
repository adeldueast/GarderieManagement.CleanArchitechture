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


       
    }
}
