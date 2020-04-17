using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class CourseAssignmentRepository : ICourseAssignmentRepository
    {
        private readonly SaveOnCloudDbContext _SaveOnCloudDbContext;

        public CourseAssignmentRepository(SaveOnCloudDbContext SaveOnCloudDbContext) => _SaveOnCloudDbContext = SaveOnCloudDbContext;

        public Task<List<CourseAssignment>> GetByCourseId(int courseId) => 
            _SaveOnCloudDbContext.CourseAssignments.Where(c => c.CourseID == courseId)
                .Include(c => c.Course)
                .Include(c => c.Instructor)
                .ToListAsync();

        public async Task<int> Add(CourseAssignment courseAssignment)
        {
            await _SaveOnCloudDbContext.CourseAssignments.AddAsync(courseAssignment);
            await _SaveOnCloudDbContext.SaveChangesAsync();
            return courseAssignment.CourseAssignmentId;
        }
    }
}
