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
    public class JournalDeBordService : IJournalDeBordService
    {
        public IJournalDeBordRepository _journalRepository { get; }

        public JournalDeBordService(IJournalDeBordRepository journalDeBordRepository)
        {
            _journalRepository = journalDeBordRepository;
        }
        public async Task<Result<JournalDeBord>> createJournal(string userId, JournalDeBord journal)
        {

            return await _journalRepository.createJournal(userId, journal);
            // throw new NotImplementedException();
        }

        public async Task<Result<JournalDeBord>> updateJournal(string userId, JournalDeBord journal)
        {
            return await _journalRepository.updateJournal(userId, journal);

        }

        public async Task<Result<JournalDeBord>> getTodayChildsJournal(string userId, int enfantId)
        {
            return await _journalRepository.getTodayChildsJournal(userId, enfantId);

        }

        public async Task<Result<JournalDeBord>> createGroupedJournals(string userId, JournalGroupedCreateRequest journalGroupedCreateRequest)
        {
            return await _journalRepository.createGroupedJournals(userId, journalGroupedCreateRequest);

        }

        public async Task<Result<JournalDeBord>> getJournalById(string userId, int journalId)
        {
            return await _journalRepository.getJournalById(userId, journalId);

        }
    }
}
