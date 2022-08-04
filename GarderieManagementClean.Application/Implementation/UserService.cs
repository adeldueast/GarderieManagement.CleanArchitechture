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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<ApplicationUser>> getAllChildsTutors(string userId, int enfantId)
        {
            return await _userRepository.getAllChildsTutors(userId, enfantId);
        }

        public async Task<Result<ApplicationUser>> getAllEmployee(string userId)
        {
            return await _userRepository.getAllEmployee(userId);
        }

        public async Task<Result<ApplicationUser>> getAllEmployeeWithNoGroup(string userId)
        {
            return await _userRepository.getAllEmployeeWithNoGroup(userId);

        }

        public async Task<Result<ApplicationUser>> getAllTutors(string userId)
        {
            return await _userRepository.getAllTutors(userId);
        }
    
    }
}
