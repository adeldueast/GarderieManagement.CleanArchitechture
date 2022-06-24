using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Services
{
    public interface IJournalDeBordService
    {
        public Task<Result<JournalDeBord>> createJournal(string userId, JournalDeBord journal);

        public Task<Result<JournalDeBord>> updateJournal(string userId, JournalDeBord journal);


    }
}
