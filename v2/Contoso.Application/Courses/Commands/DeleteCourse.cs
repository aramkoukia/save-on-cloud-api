using System.Threading.Tasks;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace SaveOnCloud.Application.Courses.Commands
{
    public class DeleteCourse : Record<DeleteCourse>, IRequest<Either<Error, Task<Unit>>>
    {
        public DeleteCourse(int courseId) => CourseId = courseId;
        public int CourseId { get; }
    }
}
