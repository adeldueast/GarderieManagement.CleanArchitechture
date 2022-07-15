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
    public class JournalDeBordService : IJournalDeBordService
    {
        public IJournalDeBordRepository _journalRepository { get; }

        public JournalDeBordService(IJournalDeBordRepository journalDeBordRepository)
        {
            _journalRepository = journalDeBordRepository;
        }
        public Task<Result<JournalDeBord>> createJournal(string userId, JournalDeBord journal)
        {

            return _journalRepository.createJournal(userId, journal);
           // throw new NotImplementedException();
        }

        public Task<Result<JournalDeBord>> updateJournal(string userId, JournalDeBord journal)
        {
            return _journalRepository.updateJournal(userId, journal);

        }

        public Task<Result<JournalDeBord>> getTodayChildsJournal(string userId, int enfantId)
        {
            return _journalRepository.getTodayChildsJournal(userId, enfantId);

        }
    }
}
