using System;

namespace SaveOnCloud.Web.Models.Azure
{
    public class CreateSubscriptionModel
    {
        public Guid SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientSecret{ get; set; }
    }
}