using System;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Departments.Commands
{
    public class CreateDepartment : Record<CreateDepartment>, IRequest<Either<Error, int>>
    {
        public CreateDepartment(string name, decimal budget, DateTime startDate, int administratorId)
        {
            Name = name;
            Budget = budget;
            StartDate = startDate;
            AdministratorId = administratorId;
        }

        public string Name { get; }
        public decimal Budget { get; }
        public DateTime StartDate { get; }
        public int AdministratorId { get; }
    }
}
