using Microsoft.Extensions.Configuration;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class CustomEmailSender : ICustomEmailSender
    {
        private readonly IConfiguration _configuration;

        public CustomEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            var client = new SmtpClient
            {
                Host = _configuration["EmailSettings:SmtpHost"],
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:SmtpUsername"],
                    _configuration["EmailSettings:SmtpPassword"])
            };

            var message = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(email);

            await client.SendMailAsync(message);
        }
    }
}
