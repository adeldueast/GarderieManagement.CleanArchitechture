using Contracts.Dtos;
using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Infrastructure.Repositories.EnfantRepository
{
    public class EnfantRepository : IEnfantRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EnfantRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<Enfant>> createEnfant(string userId, EnfantCreateRequest newEnfant)
        {
            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }
            if (!newEnfant.Tutors.Any())
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { $"At least one tutor required" }
                };
            }

            Group group = null;
            List<(ApplicationUser, string)> tutors = new List<(ApplicationUser, string)>();


            //check if group exist
            if (newEnfant.GroupId != null && newEnfant.GroupId != 0)
            {
                group = await _context.Groups.SingleOrDefaultAsync(x => x.GarderieId == user.GarderieId && x.Id == newEnfant.GroupId);
                if (group == null)
                {
                    return new Result<Enfant>
                    {
                        Errors = new List<string>() { $"Group '{newEnfant.GroupId}' doesnt exist" }
                    };
                }

            }

            //Checks for duplicate userId in the request
            var Temp = Guid.NewGuid().ToString();
            foreach (var tutorPair in newEnfant.Tutors)
            {
                var tutorId = tutorPair.TutorId;
                if (tutorId == Temp)
                {
                    return new Result<Enfant>
                    {
                        Errors = new List<string>() { $"Duplicate user '{tutorId}'" }
                    };
                }

                Temp = tutorId;

            }

            //Check if usersIds are valid 
            foreach (var tutorPair in newEnfant.Tutors)
            {

                var tutorDb = await _context.Users.SingleOrDefaultAsync(x => x.Id == tutorPair.TutorId);
                if (tutorDb == null || !await _userManager.IsInRoleAsync(tutorDb, "tutor"))
                {
                    return new Result<Enfant>
                    {
                        Errors = new List<string>() { $"Tutor '{tutorPair.TutorId}' doesnt exist or is not a tutor" }
                    };
                }
                tutors.Add(new(tutorDb, tutorPair.Relation));
            }

            var enfant = new Enfant()
            {
                Nom = newEnfant.Nom,
                DateNaissance = newEnfant.DateNaissance,
                Photo = newEnfant.Photo,
                Group = group,
            };




            await _context.Enfants.AddAsync(enfant);

            foreach (var tutorPair in tutors)
            {
                var TutorEnfant = new TutorEnfant
                {
                    Enfant = enfant,
                    ApplicationUser = tutorPair.Item1,
                    Relation = tutorPair.Item2
                };
                enfant.Tutors.Add(TutorEnfant);
            }

            await _context.SaveChangesAsync();





            return new Result<Enfant>
            {
                Success = true,
                Data = enfant,
            };
        }

        public async Task<Result<Enfant>> deleteEnfant(string userId, int EnfantId)
        {
            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }

            var enfant = await _context.Enfants.Include("Group").SingleOrDefaultAsync(x => x.Id == EnfantId && x.Group.GarderieId == user.GarderieId);
            if (enfant == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { $"Enfant '{EnfantId}' doesnt exist" }
                };
            }

            _context.Remove(enfant);
            await _context.SaveChangesAsync();


            return new Result<Enfant>
            {
                Success = true,
            };
        }

        public async Task<Result<Enfant>> getAllEnfants(string userId)
        {
            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }

            var enfants = await _context.Enfants.Include("Group").Where(x => x.Group.GarderieId == user.GarderieId).ToListAsync();


            return new Result<Enfant>
            {
                Success = true,
                Data = enfants
            };
        }

        public async Task<Result<Enfant>> getEnfantById(string userId, int EnfantId)
        {
            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }

            var enfant = await _context.Enfants.Include("Group").FirstOrDefaultAsync(x => x.Group.GarderieId == user.GarderieId && x.Id == EnfantId);

            return new Result<Enfant>
            {
                Success = true,
                Data = enfant
            };
        }

        public async Task<Result<Enfant>> updateEnfant(string userId, Enfant updatedEnfant)
        {
            var user = await getUserById(userId);
            if (user == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User not found" }
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { "User doesnt have a garderie" }
                };
            }

            var enfant = await _context.Enfants.Include("Group").FirstOrDefaultAsync(x => x.Group.GarderieId == user.GarderieId && x.Id == updatedEnfant.Id);
            if (enfant == null)
            {
                return new Result<Enfant>
                {
                    Errors = new[] { "Enfant doenst exist" }
                };
            }

            return new Result<Enfant>
            {
                Success = true,
                Data = enfant,
            };

        }

        public async Task<ApplicationUser> getUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
