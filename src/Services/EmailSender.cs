using System;
using System.IO;
using System.Linq;
using MimeKit;
using MailKit.Security;
using SaveOnCloudApi.Models;
using Microsoft.Extensions.Logging;

namespace SaveOnCloudApi.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationContext _context;
        private readonly ILogger _logger;

        public EmailSender(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<EmailSender>();
        }

        public void SendEmail(string toEmail, string subject, string textMessage, Stream attachment = null, string attachmentName = null)
        {
            try
            {
                var settings = _context.Settings.FirstOrDefault();
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(settings.FromEmail));
                message.To.Add(new MailboxAddress(toEmail));
                message.Subject = subject;
                var builder = new BodyBuilder
                {
                    TextBody = textMessage
                };

                if (!string.IsNullOrEmpty(attachmentName))
                {
                    builder.Attachments.Add(attachmentName, attachment);
                }

                message.Body = builder.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(settings.FromEmail, settings.FromEmailPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Sending Email", ex);
            }
        }
    }
}
