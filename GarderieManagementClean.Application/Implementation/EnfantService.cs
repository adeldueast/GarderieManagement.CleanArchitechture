using Contracts.Dtos.Request;
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
    public class EnfantService : IEnfantService
    {
        public IEnfantRepository _enfantRepository { get; }
        public EnfantService(IEnfantRepository enfantRepository)
        {
            _enfantRepository = enfantRepository;
        }

        public Task<Result<Enfant>> getEnfantById(string userId, int EnfantId)
        {
            return _enfantRepository.getEnfantById(userId, EnfantId);

        }

        public Task<Result<Enfant>> getAllEnfants(string userId)
        {
            return _enfantRepository.getAllEnfants(userId);
        }

        public Task<Result<Enfant>> createEnfant(string userId, EnfantCreateRequest newEnfant)
        {
            return _enfantRepository.createEnfant(userId, newEnfant);
        }

        public Task<Result<Enfant>> updateEnfant(string userId, Enfant updatedEnfant)
        {
            return _enfantRepository.updateEnfant(userId, updatedEnfant);
        }

        public Task<Result<Enfant>> deleteEnfant(string userId, int EnfantId)
        {
            return _enfantRepository.deleteEnfant(userId, EnfantId);
        }
    }
}
