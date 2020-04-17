using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface IInstructorRepository
    {
        Task<Option<Instructor>> Get(int id);
        Task<int> Add(Instructor instructor);
        Task Update(Instructor instructor);
        Task Delete(int id);
    }
}
