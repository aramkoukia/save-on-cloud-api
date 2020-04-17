using System.Threading;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using MediatR;
using static SaveOnCloud.Application.Students.Mapper;

namespace SaveOnCloud.Application.Students.Queries
{
    public class GetStudentByIdHandler : IRequestHandler<GetStudentById, Option<StudentViewModel>>
    {
        private readonly IStudentRepository _studentRepository;
        public GetStudentByIdHandler(IStudentRepository studentRepository) => 
            _studentRepository = studentRepository;

        public Task<Option<StudentViewModel>> Handle(GetStudentById request, CancellationToken cancellationToken) =>
            _studentRepository.Get(request.StudentId)
                .MapT(ProjectToViewModel);
    }
}
