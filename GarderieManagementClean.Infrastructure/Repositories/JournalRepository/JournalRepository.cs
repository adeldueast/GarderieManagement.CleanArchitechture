using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Interfaces.Services;
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
        private readonly INotificationService _notificationService;

        public JournalRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context, INotificationService notificationService)
        {
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Result<JournalDeBord>> createGroupedJournals(string userId, JournalGroupedCreateRequest journalGroupedCreateRequest)
        {
            var user = await _userManager.FindByIdAsync(userId);

            ICollection<Enfant> allEnfants = new List<Enfant>();

            foreach (var enfant_rating in journalGroupedCreateRequest.Ratings)
            {
                var enfant = await _context.Enfants.AsNoTracking().SingleOrDefaultAsync(e => e.Id == enfant_rating.Id && e.GarderieId == user.GarderieId);
                if (enfant == null)
                {
                    return new Result<JournalDeBord>()
                    {
                        Errors = new string[] { $"Enfant '{enfant_rating.Id}' doesnt not exist" }
                    };
                }

                var existingJournal = await _context.JournalDeBords.SingleOrDefaultAsync(j => j.EnfantId == enfant.Id && j.CreatedAt.Date == DateTime.Now.Date);
                if (existingJournal == null)
                {
                    var newJournal = new JournalDeBord()
                    {
                        EnfantId = enfant.Id,
                        CreatedAt = DateTime.Now,

                        Humeur_Rating = enfant_rating.Humeur_Rating,
                        Manger_Rating = enfant_rating.Manger_Rating,
                        Participation_Rating = enfant_rating.Participation_Rating,
                        Toilette_Rating = enfant_rating.Toilette_Rating,

                        Activite_Message = journalGroupedCreateRequest.Activite_Message,
                        Manger_Message = journalGroupedCreateRequest.Manger_Message,


                    };

                    _context.JournalDeBords.Add(newJournal);

                    continue;
                }

                existingJournal.LastUpdatedAt = DateTime.Now;
                existingJournal.LastUpdatedBy = user.Email;

                existingJournal.Humeur_Rating = enfant_rating.Humeur_Rating;
                existingJournal.Manger_Rating = enfant_rating.Manger_Rating;
                existingJournal.Participation_Rating = enfant_rating.Participation_Rating;
                existingJournal.Toilette_Rating = enfant_rating.Toilette_Rating;

                existingJournal.Activite_Message = journalGroupedCreateRequest.Activite_Message;
                existingJournal.Manger_Message = journalGroupedCreateRequest.Manger_Message;
            }



            await _context.SaveChangesAsync();

            return new Result<JournalDeBord>()
            {
                Success = true,

            };

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

            //After Creating a journal, we also create a notification 

            var tutors = enfant.Tutors.Select(t => t.ApplicationUser);
            Notification notification = new Notification
            {
                CreatedAt = DateTime.Now.Date,
                ApplicationUsers = new List<ApplicationUser>(tutors),
                NotificationType = NotificationTypes.Journal,
                Message = $"New journal disponible for {enfant.Nom}",

            };
            await _notificationService.createNotification(notification);


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


            //After Creating a journal, we also create a notification 
            var enfant = existingJournal.Enfant;
            var tutors = enfant.Tutors.Select(t => t.ApplicationUser);
            Notification notification = new Notification
            {
                CreatedAt = DateTime.Now.Date,
                ApplicationUsers = new List<ApplicationUser>(tutors),
                NotificationType = NotificationTypes.Journal,
                Message = $"Journal of {enfant.Nom} modified",

            };
            await _notificationService.createNotification(notification);

            return new Result<JournalDeBord>()
            {
                Success = true,
                Data = updatedJournal
            };

        }
    }
}
