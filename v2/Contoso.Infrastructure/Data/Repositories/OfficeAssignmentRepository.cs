using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class OfficeAssignmentRepository : IOfficeAssignmentRepository
    {
        private readonly SaveOnCloudDbContext SaveOnCloudDbContext;

        public OfficeAssignmentRepository(SaveOnCloudDbContext dbContext) => SaveOnCloudDbContext = dbContext;

        public Task<int> Create(OfficeAssignment officeAssignment)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Option<OfficeAssignment>> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Option<OfficeAssignment>> GetByInstructorId(int instructorId) => 
            await SaveOnCloudDbContext.OfficeAssignments
                .SingleOrDefaultAsync(o => o.InstructorId == instructorId);

        public Task Update(OfficeAssignment officeAssignment)
        {
            throw new NotImplementedException();
        }
    }
}
