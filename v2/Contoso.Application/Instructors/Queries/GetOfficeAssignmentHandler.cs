using System.Threading;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Instructors.Queries
{
    public class GetOfficeAssignmentHandler : IRequestHandler<GetOfficeAssignment, Option<OfficeAssignment>>
    {
        private readonly IOfficeAssignmentRepository _officeAssignmentRepository;

        public GetOfficeAssignmentHandler(IOfficeAssignmentRepository officeAssignmentRepository) => _officeAssignmentRepository = officeAssignmentRepository;

        public Task<Option<OfficeAssignment>> Handle(GetOfficeAssignment request, CancellationToken cancellationToken) =>
            Fetch(request.InstructorId);

        private Task<Option<OfficeAssignment>> Fetch(int id) => 
            _officeAssignmentRepository.GetByInstructorId(id);
    }
}
