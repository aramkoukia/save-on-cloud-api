using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloudApi.Models
{
    public partial class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        public virtual DbSet<Settings> Settings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            // modelBuilder.Entity<IdentityRoleClaim>().ToTable("Roles");
            // modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            // modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            // modelBuilder.Entity<IdentityUserLogin>().ToTable("Userlogins");
        }
    }
}
