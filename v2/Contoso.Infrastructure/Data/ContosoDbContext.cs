using Ardalis.EFCore.Extensions;
using SaveOnCloud.Core.Domain;
using Microsoft.EntityFrameworkCore;
using SaveOnCloud.Core.Domain.Azure;

namespace SaveOnCloud.Infrastructure.Data
{
    public class SaveOnCloudDbContext : DbContext
    {
        public SaveOnCloudDbContext(DbContextOptions<SaveOnCloudDbContext> options)
            : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }
    }
}
