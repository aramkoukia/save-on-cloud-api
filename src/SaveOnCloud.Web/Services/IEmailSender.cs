using System.IO;

namespace SaveOnCloud.Web.Services
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail, string subject, string textMessage = null, Stream attachment = null, string attachmentName = null);
    }
}
