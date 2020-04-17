using System;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Students.Commands
{
    public class CreateStudent : Record<CreateStudent>, IRequest<Either<Error, int>>
    {
        public CreateStudent(string firstName, string lastName, DateTime enrollmentDate)
        {
            FirstName = firstName;
            LastName = lastName;
            EnrollmentDate = enrollmentDate;
        }

        public string FirstName { get;  }
        public string LastName { get; }
        public DateTime EnrollmentDate { get; }
    }
}
