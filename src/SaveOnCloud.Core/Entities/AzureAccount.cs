using SaveOnCloud.SharedKernel;
using System;

namespace SaveOnCloud.Core.Entities
{
    public class AzureAccount : BaseEntity
    {
        public Guid SubscriptionId { get; set; }
        public Guid ClientId { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
        public int CompanyId { get; set; }
    }
}
