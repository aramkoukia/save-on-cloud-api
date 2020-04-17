using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Instructors.Queries
{
    public class GetInstructorById : Record<GetInstructorById>, IRequest<Option<InstructorViewModel>>
    {
        public GetInstructorById(int id) => InstructorId = id;

        public int InstructorId { get; }
    }
}
