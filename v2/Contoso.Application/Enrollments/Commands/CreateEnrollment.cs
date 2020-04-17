using SaveOnCloud.Core;
using SaveOnCloud.Core.Domain;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Enrollments.Commands
{
    public class CreateEnrollment : Record<CreateEnrollment>, IRequest<Validation<Error, int>>
    {
        public CreateEnrollment(int courseId, int studentId, Grade? grade, Course course, Student student)
        {
            CourseId = courseId;
            StudentId = studentId;
            Grade = grade;
            Course = course;
            Student = student;
        }

        public int CourseId { get; }
        public int StudentId { get; }
        public Grade? Grade { get; }
        public Course Course { get; }
        public Student Student { get; }
    }
}
