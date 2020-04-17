using System.Threading.Tasks;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Departments.Commands
{
    public class DeleteDepartment : Record<DeleteDepartment>, IRequest<Either<Error, Task>>
    {
        public DeleteDepartment(int departmentId) => DepartmentId = departmentId;

        public int DepartmentId { get; }
    }
}
