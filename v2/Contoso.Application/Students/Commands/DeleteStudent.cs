using System.Threading.Tasks;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Students.Commands
{
    public class DeleteStudent : Record<DeleteStudent>, IRequest<Either<Error, Task>>
    {
        public DeleteStudent(int studentId) => StudentId = studentId;

        public int StudentId { get; }
    }
}
