using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Instructors.Commands
{
    public class CreateOfficeAssignment: Record<CreateOfficeAssignment>, IRequest<Validation<Error, int>>
    {
        public CreateOfficeAssignment(int instructorId, string location)
        {
            InstructorId = instructorId;
            Location = location;
        }

        public int InstructorId { get; }
        public string Location { get; }
    }
}
