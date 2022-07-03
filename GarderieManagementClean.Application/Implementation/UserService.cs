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

        public Task<Result<ApplicationUser>> getAllChildsTutors(string userId, int enfantId)
        {
            return _userRepository.getAllChildsTutors(userId, enfantId);
        }

        public Task<Result<ApplicationUser>> getAllEmployee(string userId)
        {
            return _userRepository.getAllEmployee(userId);
        }

        public Task<Result<ApplicationUser>> getAllTutors(string userId)
        {
            return _userRepository.getAllTutors(userId);
        }
    }
}
