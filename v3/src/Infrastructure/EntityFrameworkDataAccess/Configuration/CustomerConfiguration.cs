// <copyright file="CustomerConfiguration.cs" company="Ivan Paulovich">
// Copyright © Ivan Paulovich. All rights reserved.
// </copyright>

namespace Infrastructure.EntityFrameworkDataAccess.Configuration
{
    using Domain.Customers.ValueObjects;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    ///     Customer Configuration.
    /// </summary>
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        /// <summary>
        ///     Configure Customer.
        /// </summary>
        /// <param name="builder">Builder.</param>
        public void Configure(EntityTypeBuilder
            <Customer> builder)
        {
            builder.ToTable("Customer");

            builder.Property(b => b.SSN)
                .HasConversion(
                    v => v.ToString(),
                    v => new SSN(v))
                .IsRequired();

            builder.Property(b => b.Name)
                .HasConversion(
                    v => v.ToString(),
                    v => new Name(v))
                .IsRequired();

            builder.Property(b => b.Id)
                .HasConversion(
                    v => v.ToGuid(),
                    v => new CustomerId(v))
                .IsRequired();

            builder.Ignore(p => p.Accounts);
        }
    }
}
