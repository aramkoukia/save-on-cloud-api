using System.Threading.Tasks;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Instructors.Commands
{
    public class DeleteInstructor : Record<DeleteInstructor>, IRequest<Either<Error, Task>>
    {
        public DeleteInstructor(int instructorId) => InstructorId = instructorId;

        public int InstructorId { get; }
    }
}
