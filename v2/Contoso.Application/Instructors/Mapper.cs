using System.Collections.Generic;
using SaveOnCloud.Core.Domain;
using LanguageExt;

namespace SaveOnCloud.Application.Instructors
{
    internal class Mapper
    {
        internal static InstructorViewModel Project(Instructor instructor) =>
            new InstructorViewModel(instructor.InstructorId, instructor.FirstName,
                instructor.LastName, instructor.HireDate,
                new Lst<CourseAssignmentViewModel>(ToViewModel(instructor.CourseAssignments)));

        private static IEnumerable<CourseAssignmentViewModel> ToViewModel(IEnumerable<CourseAssignment> courseAssignments) =>
            courseAssignments.Map(ca => Project(ca.Course));

        private static CourseAssignmentViewModel Project(Course course) =>
            new CourseAssignmentViewModel(course.CourseId, course.Title, course.Credits, course.DepartmentId);

    }
}
