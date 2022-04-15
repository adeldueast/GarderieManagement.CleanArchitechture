using GarderieManagementClean.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GarderieManagementClean.Application.Implementation
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string token, string userId)
        {
            string fromMail = "";
            string fromPassword = "";

            MailMessage message = new MailMessage()
            {
                From = new MailAddress(fromMail),
                Subject = subject,
                Body = $"<html><body> " +

                $"<p> Please click the button below to confirm your email/accept invitation</p> <br>" +

                $"<a  style=\"color:white;background-color: green; text-align: center; padding: 0 1rem 0 1rem \" " +
                $"href=\"https://localhost:44356/api/v1/Account/ConfirmEmail?userId={userId}&token={token}\" > confirm email </a>" +

                $"</body></html>",
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(toEmail));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);

            smtpClient.Dispose();

        }
    }
}
