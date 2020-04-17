using System.Threading;
using System.Threading.Tasks;
using SaveOnCloud.Core;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Instructors.Commands
{
    public class DeleteInstructorHandler : IRequestHandler<DeleteInstructor, Either<Error, Task>>
    {
        private readonly IInstructorRepository _instructorRepository;

        public DeleteInstructorHandler(IInstructorRepository instructorRepository) => 
            _instructorRepository = instructorRepository;

        public async Task<Either<Error, Task>> Handle(DeleteInstructor request, CancellationToken cancellationToken) =>
            (await InstructorMustExist(request))
                .Map(DoDeletion)
                .ToEither<Task>();

        private Task DoDeletion(int instructorId) =>
            _instructorRepository.Delete(instructorId);

        private async Task<Validation<Error, int>> InstructorMustExist(DeleteInstructor request) =>
            (await _instructorRepository.Get(request.InstructorId))
                .Map(i => i.InstructorId)
                .ToValidation<Error>($"Instructor with id {request.InstructorId} does not exist");
    }
}
