using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Repositories
{
    public interface IGarderieRepository
    {
        public Task<Result<Garderie>> getGarderie(string userId);

        public Task<Result<Garderie>> createGarderie(string userId, Garderie newGarderie);
        public Task<Result<Garderie>> updateGarderie(string userId, Garderie updatedGarderie);
        public Task<Result<Garderie>> deleteGarderie(string userId);

    }
}
