using System.Threading.Tasks;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Enrollments.Commands
{
    public class DeleteEnrollment : Record<DeleteEnrollment>, IRequest<Validation<Error, Task>>
    {
        public DeleteEnrollment(int enrollmentId) => EnrollmentId = enrollmentId;

        public int EnrollmentId { get; }
    }
}
