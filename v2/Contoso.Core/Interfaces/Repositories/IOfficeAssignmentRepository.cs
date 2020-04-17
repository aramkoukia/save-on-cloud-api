using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface IOfficeAssignmentRepository
    {
        Task<Option<OfficeAssignment>> Get(int id);
        Task<Option<OfficeAssignment>> GetByInstructorId(int instructorId);
        Task<int> Create(OfficeAssignment officeAssignment);
        Task Update(OfficeAssignment officeAssignment);
        Task Delete(int id);
    }
}
