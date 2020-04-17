namespace SaveOnCloud.Web.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string AdminEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailPassword { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
        public bool SmtpUseSsl { get; set; }
    }
}
