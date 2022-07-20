using Contracts.Dtos.Request;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Repositories
{
    public interface IJournalDeBordRepository
    {
        public Task<Result<JournalDeBord>> createJournal(string userId,JournalDeBord journal);

        public Task<Result<JournalDeBord>> updateJournal(string userId, JournalDeBord journal);

        public Task<Result<JournalDeBord>> getTodayChildsJournal(string userId, int enfantId);

        public Task<Result<JournalDeBord>> createGroupedJournals(string userId, JournalGroupedCreateRequest journalGroupedCreateRequest);


    }
}
