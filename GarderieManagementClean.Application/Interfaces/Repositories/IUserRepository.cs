using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<Result<ApplicationUser>> getAllEmployee(string userId);

        public Task<Result<ApplicationUser>> getAllTutors(string userId);
    }
}
