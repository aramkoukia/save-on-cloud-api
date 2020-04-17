using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<Option<Enrollment>> Get(int id);
        Task<int> Add(Enrollment enrollment);
        Task Update(Enrollment enrollment);
        Task Delete(int id);
    }
}
