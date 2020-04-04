using System.IO;

namespace SaveOnCloudApi.Services
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail, string subject, string textMessage = null, Stream attachment = null, string attachmentName = null);
    }
}
