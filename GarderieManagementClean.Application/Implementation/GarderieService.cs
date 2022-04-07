using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Implementation
{
    public class GarderieService : IGarderieService
    {
        public IGarderieRepository _garderieRepository { get; }
        public GarderieService(IGarderieRepository garderieRepository)
        {
            _garderieRepository = garderieRepository;
        }


        public async Task<Result<Garderie>> getGarderie(string userId)
        {
            var result = await _garderieRepository.getGarderie(userId);
            return result;
        }


        public async Task<Result<Garderie>> createGarderie(string userId, Garderie newGarderie)
        {
            var result = await _garderieRepository.createGarderie(userId, newGarderie);
            return result;

        }

     

        public async Task<Result<Garderie>> updateGarderie(string userId, Garderie updatedGarderie)
        {
            var result = await _garderieRepository.updateGarderie(userId, updatedGarderie);
            return result;
        }

        public async Task<Result<Garderie>> deleteGarderie(string userId)
        {
            var result = await _garderieRepository.deleteGarderie(userId);
            return result;
        }
    }
}
