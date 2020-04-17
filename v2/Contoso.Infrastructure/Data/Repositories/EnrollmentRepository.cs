using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly SaveOnCloudDbContext _SaveOnCloudDbContext;

        public EnrollmentRepository(SaveOnCloudDbContext SaveOnCloudDbContext) => _SaveOnCloudDbContext = SaveOnCloudDbContext;

        public async Task<int> Add(Enrollment enrollment)
        {
            _SaveOnCloudDbContext.Enrollments.Add(enrollment);
            await _SaveOnCloudDbContext.SaveChangesAsync();
            return enrollment.EnrollmentId;
        }

        public async Task Delete(int id)
        {
            var enrollment = await _SaveOnCloudDbContext.Enrollments.FindAsync(id);
            _SaveOnCloudDbContext.Enrollments.Remove(enrollment);
            await _SaveOnCloudDbContext.SaveChangesAsync();
        }

        public async Task<Option<Enrollment>> Get(int id) => 
            await _SaveOnCloudDbContext.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .SingleOrDefaultAsync(e => e.EnrollmentId == id);

        public async Task Update(Enrollment enrollment)
        {
            _SaveOnCloudDbContext.Enrollments.Update(enrollment);
            await _SaveOnCloudDbContext.SaveChangesAsync();
        }
    }
}
