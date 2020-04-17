using System;

namespace SaveOnCloud.Core.Domain.Azure
{
    public class Subscription
    {
        public Guid SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientPassword { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
