using GarderieManagementClean.Application.Interfaces.Repositories;
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


    public class GroupService : IGroupService
    {

        public IGroupRepository _groupRepository { get; }
        public Task<Result<Group>> createGroup(string userId, Group newGroup)
        {
            return _groupRepository.createGroup(userId, newGroup);
        }

        public Task<Result<Group>> deleteGroup(string userId, int GroupId)
        {
            return _groupRepository.deleteGroup(userId, GroupId);
        }

        public Task<Result<Group>> getAllGroups(string userId)
        {
            return _groupRepository.getAllGroups(userId);
        }

        public Task<Result<Group>> getGroupById(string userId, int GroupId)
        {
            return _groupRepository.getGroupById(userId,GroupId);
        }

        public Task<Result<Group>> updateGroup(string userId, Group updatedGroup)
        {
            return _groupRepository.updateGroup(userId, updatedGroup);
        }
    }
}
