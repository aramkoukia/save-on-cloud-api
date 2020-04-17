using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Departments.Queries
{
    public class GetDepartmentById : Record<GetDepartmentById>, IRequest<Option<DepartmentViewModel>>
    {
        public GetDepartmentById(int departmentId) => DepartmentId = departmentId;

        public int DepartmentId { get; }
    }
}
