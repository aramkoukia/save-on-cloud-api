using System.Collections.Generic;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Core.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<Option<Student>> Get(int Id);
        Task<List<Student>> GetAll();
        Task<int> Add(Student student);
        Task Update(Student student);
        Task Delete(int id);
    }
}
