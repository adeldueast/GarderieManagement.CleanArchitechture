﻿using Contracts.Dtos.Response;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Identity;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Infrastructure.Repositories.GroupRepository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GroupRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<Group>> createGroup(string userId, Group newGroup)
        {
            var user = await getUserById(userId);


            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt create group because user '{userId}' doesnt have a garderie" }
                };
            }

            //TODO: TEST THIS
            //Check if educatrice exist
            var educatrice = await _userManager.FindByIdAsync(newGroup.EducatriceId);


            if (educatrice == null || educatrice.GarderieId != user.GarderieId)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt create group because user '{newGroup.EducatriceId}' was not found" }
                };
            }


            //this is not null, but the column GroupId is NULL 
            var group = educatrice.Group;





            //Check if educatrice already has a group
            if (educatrice.Group != null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt create group because user '{userId}' already has a group" }
                };
            }


            //Check if educatrice is in role emplyee
            if (!await _userManager.IsInRoleAsync(educatrice, "employee"))
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt create group because user '{userId}' is not an employee" }
                };
            }


            newGroup.GarderieId = (int)user.GarderieId;
            newGroup.ApplicationUser = educatrice;


            _context.Add(newGroup);

            await _context.SaveChangesAsync();




            await _context.SaveChangesAsync();

            return new Result<Group>
            {
                Success = true,
                Data = newGroup,
            };

        }

        public async Task<Result<Group>> deleteGroup(string userId, int GroupId)
        {

            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt delete group because user '{userId}' was not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt delete group because user '{userId}' doesnt have a garderie" }
                };
            }


            var group = _context.Groups.Include(g => g.Enfants).SingleOrDefault(g => g.GarderieId == (int)user.GarderieId && g.Id == GroupId);

            if (group == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt delete group because group '{GroupId}' doesnt exist" }
                };
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return new Result<Group>
            {
                Success = true,
                Data = new { Message = $"Successfully deleted group '{GroupId}'" }
            };

        }

        public async Task<Result<Group>> getAllGroups(string userId)
        {
            var user = await getUserById(userId);



            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get groups because user '{userId}' doesnt have a garderie" }
                };
            }


            var groups = await _context.Groups.Where(g => g.GarderieId == user.GarderieId)
                .Select(g => new
                {
                    Id = g.Id,
                    Name = g.Name,
                    HexColor = g.HexColor,
                    EducatriceFullName = $"{g.ApplicationUser.FirstName} {g.ApplicationUser.LastName}",
                    //list of childs that are actually present (has arrived)
                    Enfants = g.Enfants.Where(e => e.Attendances.Any(attendance => attendance.ArrivedAt.Value.Date == DateTime.Now.Date && !attendance.LeftAt.HasValue))
                    .Select(x =>new
                    {
                        Image  = x.PhotoCouverture != null ? x.PhotoCouverture.Id.ToString() : null,
                        Nom = x.Nom
                    }).ToList(),
                    //hasArrived = x.Attendances.Any(attendance => attendance.ArrivedAt.Value.Date == DateTime.Now.Date && !attendance.LeftAt.HasValue),

                })
                .ToListAsync();

            return new Result<Group>
            {
                Success = true,
                Data = groups
            };
        }

        public async Task<Result<Group>> getGroupById(string userId, int GroupId)
        {
            var user = await getUserById(userId);

            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get group because user '{userId}' doesnt have a garderie" }
                };
            }

            var group = await _context.Groups
                .SingleOrDefaultAsync(g => g.GarderieId == user.GarderieId && g.Id == GroupId);


            if (group is null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get group '{GroupId}' because it doesnt exist" }
                };
            }


            return new Result<Group>
            {
                Success = true,
                Data = group
            };
        }

        public async Task<Result<Group>> updateGroup(string userId, Group updatedGroup)
        {
            var user = await getUserById(userId);

            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt update group because user '{userId}' doesnt have a garderie" }
                };
            }

            var group = _context.Groups.SingleOrDefault(g => g.GarderieId == user.GarderieId && g.Id == updatedGroup.Id);

            if (group is null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt update group '{updatedGroup.Id}' because it doesnt exist" }
                };
            }


            group.Name = updatedGroup.Name;
            group.Photo = updatedGroup.Photo;
            group.EducatriceId = updatedGroup.EducatriceId;

            await _context.SaveChangesAsync();

            return new Result<Group>
            {
                Success = true,
                Data = group
            };

        }

        #region Helper methods

        public async Task<ApplicationUser> getUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        #endregion
    }
}
