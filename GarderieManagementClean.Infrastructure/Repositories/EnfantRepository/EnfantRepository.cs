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
            //if (!newEnfant.Tutors.Any())
            //{
            //    return new Result<Enfant>
            //    {
            //        Errors = new List<string>() { $"At least one tutor required" }
            //    };
            //}

            // Group group = null;
            //List<(ApplicationUser, string)> tutors = new List<(ApplicationUser, string)>();


            ////check if group exist
            //if (newEnfant.GroupId != null && newEnfant.GroupId != 0)
            //{
            //    group = await _context.Groups.SingleOrDefaultAsync(x => x.GarderieId == user.GarderieId && x.Id == newEnfant.GroupId);
            //    if (group == null)
            //    {
            //        return new Result<Enfant>
            //        {
            //            Errors = new List<string>() { $"Group '{newEnfant.GroupId}' doesnt exist" }
            //        };
            //    }
            //}
            //Checks for duplicate userId in the request
            //var Temp = Guid.NewGuid().ToString();
            //foreach (var tutorPair in newEnfant.Tutors)
            //{
            //    var tutorId = tutorPair.TutorEmail;
            //    if (tutorId == Temp)
            //    {
            //        return new Result<Enfant>
            //        {
            //            Errors = new List<string>() { $"Duplicate user '{tutorId}'" }
            //        };
            //    }
            //    Temp = tutorId;
            //}

            ////Check if usersIds are valid 
            //foreach (var tutorPair in newEnfant.Tutors)
            //{
            //    //var tutorDb = await _context.Users.SingleOrDefaultAsync(x => x.Id == tutorPair.TutorEmail);
            //    var tutorDb = await _userManager.FindByEmailAsync(tutorPair.TutorEmail);
            //    if (tutorDb == null || !await _userManager.IsInRoleAsync(tutorDb, "tutor"))
            //    {
            //        return new Result<Enfant>
            //        {
            //            Errors = new List<string>() { $"Tutor '{tutorPair.TutorEmail}' doesnt exist or is not a tutor" }
            //        };
            //    }
            //    tutors.Add(new(tutorDb, tutorPair.Relation));
            //}

            var enfant = new Enfant()
            {
                Nom = newEnfant.Nom,
                DateNaissance = newEnfant.DateNaissance,
                GarderieId = (int)user.GarderieId
            };

            _context.Enfants.Add(enfant);
            //foreach (var tutorPair in tutors)
            //{
            //    var TutorEnfant = new TutorEnfant
            //    {
            //        Enfant = enfant,
            //        ApplicationUser = tutorPair.Item1,
            //        Relation = tutorPair.Item2
            //    };
            //    enfant.Tutors.Add(TutorEnfant);
            //}

            await _context.SaveChangesAsync();



            return new Result<Enfant>
            {
                Success = true,
                Data = enfant,
            };
        }

        public async Task<Result<Enfant>> updateEnfant(string userId, EnfantUpdateRequest updatedEnfant)
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

            var enfant = await _context.Enfants.FirstOrDefaultAsync(x => x.GarderieId == user.GarderieId && x.Id == updatedEnfant.Id);
            if (enfant == null)
            {

                return new Result<Enfant>
                {
                    Errors = new[] { $"Enfant '{updatedEnfant.Id}' doenst exist" }
                };
            }


            Group group = null;
            //check if groupId is valid
            if (updatedEnfant.GroupId != null && updatedEnfant.GroupId != 0)
            {
                group = await _context.Groups.SingleOrDefaultAsync(g => g.Id == updatedEnfant.GroupId && g.GarderieId == user.GarderieId);
            }
            
          

           

           


            enfant.Nom = updatedEnfant.Nom;
            enfant.DateNaissance = updatedEnfant.DateNaissance;
            enfant.GroupId = group == null ? null : group.Id;
         
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

            var enfant = await _context.Enfants.SingleOrDefaultAsync(x => x.Id == EnfantId && x.GarderieId == user.GarderieId);
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
                Data = new { Message = $"Successfuly deleted enfant '{EnfantId}'" }
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

            var enfants = await _context.Enfants.Where(x => x.GarderieId == user.GarderieId).ToListAsync();


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

            var enfant = await _context.Enfants.FirstOrDefaultAsync(x => x.GarderieId == user.GarderieId && x.Id == EnfantId);
            if (enfant is null)
            {
                return new Result<Enfant>
                {
                    Errors = new List<string>() { $"Enfant '{EnfantId}' does not exist." }
                };
            }

            return new Result<Enfant>
            {
                Success = true,
                Data = enfant
            };
        }

        public async Task<ApplicationUser> getUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<Result<Enfant>> assignTutorToEnfant(string userId, EnfantAssignTutorRequest enfantAssignTutorRequest)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);


            //Check if guardian exist
            var tutor = await _context.Users.SingleOrDefaultAsync(u =>
            u.GarderieId == currentUser.GarderieId &&
            u.Id == enfantAssignTutorRequest.TutorId
            );



            if (tutor is null)
            {
               
                return new Result<Enfant> { Errors = new List<string>() { $"Tutor '{enfantAssignTutorRequest.TutorId}' doesnt exist" } };
            }

            //Check if child exist
            var enfant = await _context.Enfants.AsNoTracking().SingleOrDefaultAsync(u =>
            u.GarderieId == currentUser.GarderieId &&
            u.Id == enfantAssignTutorRequest.EnfantId);

            if (enfant is null)
            {
                return new Result<Enfant> { Errors = new List<string>() { $"Enfant '{enfantAssignTutorRequest.EnfantId}' doesnt exist" } };
            }


            var hasRelation = tutor.Tutors.Any(te => te.EnfantId == enfant.Id);



        
            if (hasRelation)
            {
                var relation = await _context.TutorEnfant.SingleOrDefaultAsync(te => 
                te.ApplicationUserId == tutor.Id &&
                te.EnfantId == enfant.Id);
                relation.Relation = enfantAssignTutorRequest.Relation;

                // return new Result<Enfant> { Errors = new List<string>() { $"Tutor '{enfantAssignTutorRequest.TutorId}' is already a tutor of '{enfantAssignTutorRequest.EnfantId}'" } };

            }
            else
            {
                //added an existing relation makes an EXCEPTION 
                tutor.Tutors.Add(
                    new TutorEnfant()
                    {
                        ApplicationUserId = tutor.Id,
                        EnfantId = enfant.Id,
                        Relation = enfantAssignTutorRequest.Relation
                    }
                    );



            }



            await _context.SaveChangesAsync();
            return new Result<Enfant>()
            {
                Success = true,
                Data = "Assignation successful"
            };

        }
    }
}
