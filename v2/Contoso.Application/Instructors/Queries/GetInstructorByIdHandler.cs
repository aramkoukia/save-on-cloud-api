using System.Threading;
using System.Threading.Tasks;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using MediatR;
using static SaveOnCloud.Application.Instructors.Mapper;

namespace SaveOnCloud.Application.Instructors.Queries
{
    public class GetInstructorByIdHandler : IRequestHandler<GetInstructorById, Option<InstructorViewModel>>
    {
        private readonly IInstructorRepository _instructorRepository;
        public GetInstructorByIdHandler(IInstructorRepository instructorRepository) => _instructorRepository = instructorRepository;

        public Task<Option<InstructorViewModel>> Handle(GetInstructorById request, CancellationToken cancellationToken) => 
            _instructorRepository.Get(request.InstructorId).MapT(Project);
    }
}
