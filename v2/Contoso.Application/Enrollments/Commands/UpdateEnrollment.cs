using System.Threading.Tasks;
using SaveOnCloud.Core;
using SaveOnCloud.Core.Domain;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Enrollments.Commands
{
    public class UpdateEnrollment : Record<UpdateEnrollment>, IRequest<Validation<Error, Task>>
    {
        public UpdateEnrollment(int enrollmentId, Grade? grade)
        {
            EnrollmentId = enrollmentId;
            Grade = grade;
        }

        public int EnrollmentId { get; }
        public Grade? Grade { get; }
    }
}
