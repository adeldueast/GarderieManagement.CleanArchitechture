using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Services
{
    public interface IGroupService
    {
        public Task<Result<Group>> getGroupById(string userId, int GroupId);
        public Task<Result<Group>> getAllGroups(string userId);
        public Task<Result<Group>> createGroup(string userId, Group newGroup);
        public Task<Result<Group>> updateGroup(string userId, Group updatedGroup);
        public Task<Result<Group>> deleteGroup(string userId, int GroupId);
    }
}
