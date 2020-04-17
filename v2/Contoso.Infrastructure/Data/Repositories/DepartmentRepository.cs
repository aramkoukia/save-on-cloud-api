using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly SaveOnCloudDbContext SaveOnCloudDbContext;
        public DepartmentRepository(SaveOnCloudDbContext dbContext) => SaveOnCloudDbContext = dbContext;

        public async Task<int> Add(Department department)
        {
            await SaveOnCloudDbContext.Departments.AddAsync(department);
            await SaveOnCloudDbContext.SaveChangesAsync();
            return department.DepartmentId;
        }

        public async Task Delete(int departmentId)
        {
            var department = await SaveOnCloudDbContext.Departments.FindAsync(departmentId);
            SaveOnCloudDbContext.Departments.Remove(department);
            await SaveOnCloudDbContext.SaveChangesAsync();
        }

        public async Task<Option<Department>> Get(int id) => 
            await SaveOnCloudDbContext.Departments
                .SingleOrDefaultAsync(d => d.DepartmentId == id);

        public async Task Update(Department department)
        {
            SaveOnCloudDbContext.Departments.Update(department);
            await SaveOnCloudDbContext.SaveChangesAsync();
        }
    }
}
