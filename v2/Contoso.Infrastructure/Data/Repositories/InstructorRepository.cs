using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly SaveOnCloudDbContext SaveOnCloudDbContext;
        public InstructorRepository(SaveOnCloudDbContext context) => SaveOnCloudDbContext = context;

        public async Task<int> Add(Instructor instructor)
        {
            await SaveOnCloudDbContext.Instructors.AddAsync(instructor);
            await SaveOnCloudDbContext.SaveChangesAsync();
            return instructor.InstructorId;
        }

        public async Task Delete(int id)    
        {
            var instructor = await SaveOnCloudDbContext.Instructors.FindAsync(id);
            SaveOnCloudDbContext.Instructors.Remove(instructor);
            await SaveOnCloudDbContext.SaveChangesAsync();
        }

        public async Task<Option<Instructor>> Get(int id) => 
            await SaveOnCloudDbContext.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(c => c.Course)
                .SingleOrDefaultAsync(i => i.InstructorId == id);

        public async Task Update(Instructor instructor)
        {
            SaveOnCloudDbContext.Instructors.Update(instructor);
            await SaveOnCloudDbContext.SaveChangesAsync();
        }
    }
}
