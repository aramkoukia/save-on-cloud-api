
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloudApi.Models
{
    public partial class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public virtual DbSet<Settings> Settings { get; set; }
        //public SaveOnCloudContext()
        //{
        //}

        //public SaveOnCloudContext(DbContextOptions<SaveOnCloudContext> options)
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // optionsBuilder.UseSqlServer(@"connection string");
        //    }
        // }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    foreach (var entity in modelBuilder.Model.GetEntityTypes())
        //    {
        //        // Remove 'AspNet' prefix and convert table name from PascalCase to snake_case. E.g. AspNetRoleClaims -> role_claims
        //        entity.Relational().TableName = entity.Relational().TableName.Replace("AspNet", "");

        //        // Convert column names from PascalCase to snake_case.
        //        foreach (var property in entity.GetProperties())
        //        {
        //            property.Relational().ColumnName = property.Name;
        //        }

        //        // Convert primary key names from PascalCase to snake_case. E.g. PK_users -> pk_users
        //        foreach (var key in entity.GetKeys())
        //        {
        //            key.Relational().Name = key.Relational().Name;
        //        }

        //        // Convert foreign key names from PascalCase to snake_case.
        //        foreach (var key in entity.GetForeignKeys())
        //        {
        //            key.Relational().Name = key.Relational().Name;
        //        }

        //        // Convert index names from PascalCase to snake_case.
        //        foreach (var index in entity.GetIndexes())
        //        {
        //            index.Relational().Name = index.Relational().Name;
        //        }
        //    }
        //}
    }

}
