using System;
using System.IO;
using System.Linq;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using SaveOnCloud.Infrastructure.Data;
using SaveOnCloud.SharedKernel.Interfaces;

namespace SaveOnCloud.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public EmailSender(AppDbContext context, ILoggerFactory loggerFactory)
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
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = textMessage,
                    TextBody = textMessage
                };
                message.Body = bodyBuilder.ToMessageBody();

                if (!string.IsNullOrEmpty(attachmentName))
                {
                    bodyBuilder.Attachments.Add(attachmentName, attachment);
                }

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
