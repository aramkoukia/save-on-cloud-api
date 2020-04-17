using SaveOnCloud.Core.Domain;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Instructors.Queries
{
    public class GetOfficeAssignment : Record<GetOfficeAssignment>, IRequest<Option<OfficeAssignment>>
    {
        public GetOfficeAssignment(int instructorId) => InstructorId = instructorId;
        public int InstructorId { get; }
    }
}
