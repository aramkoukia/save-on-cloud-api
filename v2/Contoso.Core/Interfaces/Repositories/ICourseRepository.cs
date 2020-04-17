using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<Option<Course>> Get(int id);
        Task<int> Add(Course course);
        Task<Unit> Update(Course course);
        Task<Unit> Delete(int id);
    }
}
