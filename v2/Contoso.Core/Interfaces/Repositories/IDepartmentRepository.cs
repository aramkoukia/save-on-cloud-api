using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Option<Department>> Get(int id);
        Task<int> Add(Department department);
        Task Update(Department department);
        Task Delete(int departmentId);
    }
}
