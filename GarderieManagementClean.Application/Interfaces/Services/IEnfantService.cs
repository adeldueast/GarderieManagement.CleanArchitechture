using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Services
{
    public interface IEnfantService
    {
        public Task<Result<Enfant>> getEnfantById(string userId, int EnfantId);
        public Task<Result<Enfant>> getAllEnfants(string userId);

        public Task<Result<Enfant>> getAllTutorsEnfants(string userId);


        public Task<Result<Enfant>> getAllEnfantsGroupedByGroup(string userId);

        public Task<Result<Enfant>> createEnfant(string userId, EnfantCreateRequest newEnfant);
        public Task<Result<Enfant>> updateEnfant(string userId, EnfantUpdateRequest updatedEnfant);
        public Task<Result<Enfant>> deleteEnfant(string userId, int EnfantId);

        public Task<Result<Enfant>> assignTutorToEnfant(string userId,EnfantAssignTutorRequest enfantAssignTutorRequest);

    }
}
