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
    public  class EnfantService : IEnfantService
    {
        public  IEnfantRepository _enfantRepository { get; }
        public  EnfantService(IEnfantRepository enfantRepository)
        {
            _enfantRepository = enfantRepository;
        }

        public async Task<Result<Enfant>> getAllEnfantsGroupedByGroup(string userId)
        {
            return await  _enfantRepository.getAllEnfantsGroupedByGroup(userId);

        }

        public async Task<Result<Enfant>> getEnfantById(string userId, int EnfantId)
        {
            return await  _enfantRepository.getEnfantById(userId, EnfantId);

        }

        public async Task<Result<Enfant>> getAllEnfants(string userId)
        {
            return await  _enfantRepository.getAllEnfants(userId);
        }

        public async Task<Result<Enfant>> createEnfant(string userId, EnfantCreateRequest newEnfant)
        {
            return await  _enfantRepository.createEnfant(userId, newEnfant);
        }

        public async Task<Result<Enfant>> updateEnfant(string userId, EnfantUpdateRequest updatedEnfant)
        {
            return await  _enfantRepository.updateEnfant(userId, updatedEnfant);
        }

        public async Task<Result<Enfant>> deleteEnfant(string userId, int EnfantId)
        {
            return await  _enfantRepository.deleteEnfant(userId, EnfantId);
        }

        public async Task<Result<Enfant>> assignTutorToEnfant(string userId, EnfantAssignTutorRequest enfantAssignTutorRequest)
        {
            return await  _enfantRepository.assignTutorToEnfant(userId, enfantAssignTutorRequest);

        }

        public async Task<Result<Enfant>> getAllTutorsEnfants(string userId)
        {
            return await  _enfantRepository.getAllTutorsEnfants(userId);
        }
    }
}
