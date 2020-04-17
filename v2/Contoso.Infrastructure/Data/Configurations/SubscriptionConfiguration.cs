using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveOnCloud.Core.Domain.Azure;

namespace SaveOnCloud.Infrastructure.Data.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasOne(c => c.Organization);
            builder.Property(b => b.TenantId).HasMaxLength(250);
            builder.Property(b => b.ClientPassword).HasMaxLength(250);
        }
    }
}
