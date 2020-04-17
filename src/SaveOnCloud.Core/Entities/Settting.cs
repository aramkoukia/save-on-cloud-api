using SaveOnCloud.SharedKernel;

namespace SaveOnCloud.Core.Entities
{
    public class Setting : BaseEntity
    {
        public string AdminEmail { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailPassword { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpHost { get; set; }
        public bool SmtpUseSsl { get; set; }
    }
}
