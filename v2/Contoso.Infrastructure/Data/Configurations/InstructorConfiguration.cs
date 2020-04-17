using SaveOnCloud.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SaveOnCloud.Infrastructure.Data.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.Property(b => b.FirstName)
                .HasMaxLength(50);

            builder.Property(b => b.LastName)
                .HasMaxLength(50);
        }
    }
}
