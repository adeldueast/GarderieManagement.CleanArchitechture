﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public  Task SendEmailAsync(string email, string subject, string token, string userId);
    }


}
