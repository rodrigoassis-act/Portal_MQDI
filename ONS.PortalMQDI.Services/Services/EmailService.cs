using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class EmailService
    {
        private readonly IOptions<SmtpSettings> _smtpServiceSettings;

        public EmailService(IOptions<SmtpSettings> smtpServiceSettings)
        {
            _smtpServiceSettings = smtpServiceSettings;
        }

        public async Task SendEmailAsync(List<string> toAddresses, string subject, string body, bool isHtml = false)
        {
            var email = new MailMessage
            {
                From = new MailAddress(_smtpServiceSettings.Value.FromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var toAddress in toAddresses)
            {
                email.To.Add(new MailAddress(toAddress));
            }

            using (var client = new SmtpClient(_smtpServiceSettings.Value.SmtpServer, _smtpServiceSettings.Value.SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_smtpServiceSettings.Value.Username, _smtpServiceSettings.Value.Password);

                try
                {
                    await client.SendMailAsync(email);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Falha ao enviar e-mail.", ex);
                }
            }
        }
    }
}
