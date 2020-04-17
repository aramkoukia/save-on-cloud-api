using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface ICourseAssignmentRepository
    {
        Task<List<CourseAssignment>> GetByCourseId(int courseId);
        Task<int> Add(CourseAssignment courseAssignment);
    }
}
