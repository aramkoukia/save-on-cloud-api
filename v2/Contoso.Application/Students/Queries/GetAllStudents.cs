using System.Collections.Generic;
using SaveOnCloud.Core.Domain;
using MediatR;

namespace SaveOnCloud.Application.Students.Queries
{
    public class GetAllStudents : IRequest<List<StudentViewModel>>
    { }
}
