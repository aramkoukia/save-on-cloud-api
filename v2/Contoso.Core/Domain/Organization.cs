using SaveOnCloud.Core.Domain.Azure;
using System.Collections.Generic;

namespace SaveOnCloud.Core.Domain
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public List<Subscription> Subscription { get; set; }
    }
}
