using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Identity;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
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
            if (user == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt create group because user '{userId}' was not found" }
                };
            }

            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt create group because user '{userId}' doesnt have a garderie" }
                };
            }


            newGroup.GarderieId = (int)user.GarderieId;

            await _context.AddAsync(newGroup);
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


            var group = _context.Groups.SingleOrDefault(g => g.GarderieId == (int)user.GarderieId && g.Id == GroupId);

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

            if (user == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get groups because user '{userId}' was not found" }
                };
            }

            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get groups because user '{userId}' doesnt have a garderie" }
                };
            }



            var groups = _context.Groups.Where(g => g.GarderieId == user.GarderieId).ToList();

            return new Result<Group>
            {
                Success = true,
                Data = groups
            };
        }

        public async Task<Result<Group>> getGroupById(string userId, int GroupId)
        {
            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get group because user '{userId}' was not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt get group because user '{userId}' doesnt have a garderie" }
                };
            }

            var group = _context.Groups.SingleOrDefault(g => g.GarderieId == user.GarderieId && g.Id == GroupId);

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
            if (user == null)
            {
                return new Result<Group>
                {
                    Errors = new List<string>() { $"Couldnt update groups because user '{userId}' was not found" }
                };
            }
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
