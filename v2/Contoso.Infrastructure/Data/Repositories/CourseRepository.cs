using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SaveOnCloudDbContext SaveOnCloudDbContext;

        public CourseRepository(SaveOnCloudDbContext SaveOnCloudDbContext) => 
            this.SaveOnCloudDbContext = SaveOnCloudDbContext;

        public async Task<int> Add(Course course)
        {
            await SaveOnCloudDbContext.Courses.AddAsync(course);
            await SaveOnCloudDbContext.SaveChangesAsync();
            return course.CourseId;
        }

        public async Task<Unit> Delete(int id)
        {
            var course = await SaveOnCloudDbContext.Courses.FindAsync(id);
            SaveOnCloudDbContext.Courses.Remove(course);
            return await SaveOnCloudDbContext
                .SaveChangesAsync()
                .Map(_ => Unit.Default);
        }

        public async Task<Option<Course>> Get(int id) => 
            await SaveOnCloudDbContext.Courses.SingleOrDefaultAsync(c => c.CourseId == id);

        public Task<Unit> Update(Course course)
        {
            SaveOnCloudDbContext.Courses.Update(course);
            return SaveOnCloudDbContext.SaveChangesAsync()
                .Map(_ => Unit.Default);
        }
    }
}
