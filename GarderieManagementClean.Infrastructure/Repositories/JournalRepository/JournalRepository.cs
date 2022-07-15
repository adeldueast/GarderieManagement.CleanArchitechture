﻿using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Infrastructure.Repositories.JournalRepository
{
    public class JournalRepository : IJournalDeBordRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JournalRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<Result<JournalDeBord>> createJournal(string userId, JournalDeBord newJournal)
        {
            var user = await _userManager.FindByIdAsync(userId);


            //Check if children exist
            var enfant = _context.Enfants.SingleOrDefault(e => e.Id == newJournal.EnfantId && e.GarderieId == user.GarderieId);
            if (enfant == null)
            {
                return new Result<JournalDeBord>()
                {
                    Errors = new string[] { $"Enfant '{newJournal.EnfantId}' does not exist" },
                };
            }

            //Check if a journal was already made for today for that specific child
            var existingJournal = _context.JournalDeBords.SingleOrDefault(j =>
            j.CreatedAt.Date == newJournal.CreatedAt.Date &&
            j.EnfantId == newJournal.EnfantId
            );

            //if already exist, return error
            if (existingJournal != null)
            {
                return new Result<JournalDeBord>()
                {
                    Errors = new string[] { $"A journal was already made today for children '{newJournal.EnfantId}' " },
                };

            }

            newJournal.EducatriceId = userId;

            _context.Add(newJournal);
            await _context.SaveChangesAsync();

            return new Result<JournalDeBord>()
            {
                Success = true,
                Data = newJournal
            };






        }

        public async Task<Result<JournalDeBord>> getTodayChildsJournal(string userId, int enfantId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var enfant = await _context.Enfants.SingleOrDefaultAsync(e => e.Id == enfantId && e.GarderieId == user.GarderieId);
            if (enfant == null)
            {
                return new Result<JournalDeBord>()
                {
                    Errors = new string[] { $"Enfant '{enfantId}' doesnt exist" }
                };
            }


            var existingJournal = _context.JournalDeBords.SingleOrDefault(j =>
                j.EnfantId == enfantId &&
                j.CreatedAt.Date == DateTime.Now.Date
            );


            return new Result<JournalDeBord>()
            {
                Success = true,
                Data = existingJournal,

            };

        }

        public async Task<Result<JournalDeBord>> updateJournal(string userId, JournalDeBord updatedJournal)
        {
            var user = await _userManager.FindByIdAsync(userId);



            var existingJournal = _context.JournalDeBords.SingleOrDefault(j =>
            j.EnfantId == updatedJournal.EnfantId &&
            j.Enfant.GarderieId == user.GarderieId &&
            j.CreatedAt.Date.Date == DateTime.Now.Date
            );
            //if not found, return error
            if (existingJournal == null)
            {
                return new Result<JournalDeBord>()
                {
                    Errors = new string[] { $"Journal '{updatedJournal.Id}' doesnt exist" },
                };

            }

            existingJournal.Humeur_Rating = updatedJournal.Humeur_Rating;
            existingJournal.Manger_Rating = updatedJournal.Manger_Rating;
            existingJournal.Toilette_Rating = updatedJournal.Toilette_Rating;
            existingJournal.Participation_Rating = updatedJournal.Participation_Rating;

            existingJournal.Activite_Message = updatedJournal.Activite_Message;
            existingJournal.Manger_Message = updatedJournal.Manger_Message;
            existingJournal.Commentaire_Message = updatedJournal.Commentaire_Message;


            existingJournal.LastUpdatedAt = DateTime.Now;
            existingJournal.LastUpdatedBy = user.Email;




            _context.Update(existingJournal);
            await _context.SaveChangesAsync();

            return new Result<JournalDeBord>()
            {
                Success = true,
                Data = updatedJournal
            };

        }
    }
}
