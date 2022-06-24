using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Identity;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Infrastructure.Repositories
{
    public class GarderieRepository : IGarderieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GarderieRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<Result<Garderie>> getGarderie(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User is null" },
                    Data = null
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User not assigned to a garderie" },
                    Data = null
                };
            }

            var garderie = await _context.Garderies.FirstOrDefaultAsync(g => g.Id == user.GarderieId);
            _context.Entry(garderie).State = EntityState.Detached;
            if (garderie == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { $"Garderie '{user.GarderieId}' not found" }
                };
            }

            return new Result<Garderie>()
            {
                Success = true,
                Data = garderie,
            };
        }

        public async Task<Result<Garderie>> createGarderie(string userId, Garderie newGarderie)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User is null" },
                    Data = null
                };
            }
            if (user.GarderieId != null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User already assigned to a garderie" },
                    Data = null
                };
            }

            var garderie = new Garderie
            {
                Name = newGarderie.Name,
                Address = newGarderie.Address,
            };

             _context.Garderies.Add(newGarderie);
            user.Garderie = newGarderie;
            await _context.SaveChangesAsync();

            return new Result<Garderie>()
            {
                Success = true,
                Data = newGarderie
            };

        }

        public async Task<Result<Garderie>> updateGarderie(string userId, Garderie updatedGarderieRequest)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User is null" },
                    Data = null
                };
            }
            if (user.GarderieId == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User not assigned to a garderie" },
                    Data = null
                };
            }

            var garderieToUpdate = await _context.Garderies.FirstOrDefaultAsync(g => g.Id == user.GarderieId);
            if (garderieToUpdate == null)
            {
                return null;
            }
            // The problem is updatedGarderieRequest wich is the dto(mapped to domain Garderie) has no Id
            // so by executing next line, im assigning Id of 0 to my tracked Garderie when I want to update
            // all of its propreties to the dto Garderie

            garderieToUpdate.Name = updatedGarderieRequest.Name;
            garderieToUpdate.Address.Ville = updatedGarderieRequest.Address.Ville;
            garderieToUpdate.Address.Rue = updatedGarderieRequest.Address.Rue;
            garderieToUpdate.Address.Province = updatedGarderieRequest.Address.Province;
            garderieToUpdate.Address.CodePostal = updatedGarderieRequest.Address.CodePostal;
            garderieToUpdate.Address.Telephone = updatedGarderieRequest.Address.Telephone;


            await _context.SaveChangesAsync();

            return new Result<Garderie>()
            {
                Success = true,
                Data = garderieToUpdate,
            };
        }

        public async Task<Result<Garderie>> deleteGarderie(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User not found" },
                    Data = null
                };
            }

            if (user.GarderieId == null)
            {
                return new Result<Garderie>()
                {
                    Errors = new string[] { "User not assigned to a garderie" },
                    Data = null
                };
            }

            var users = _context.Users.Where(x => x.GarderieId == user.GarderieId);
            foreach (var u in users)
            {
                u.Garderie = null;
            }

            var garderie = await _context.Garderies.SingleOrDefaultAsync(g => g.Id == user.GarderieId);
            _context.Remove(garderie);
            await _context.SaveChangesAsync();

            return new Result<Garderie>()
            {
                Success = true,
                Data = garderie,
            };
        }
    }
}
