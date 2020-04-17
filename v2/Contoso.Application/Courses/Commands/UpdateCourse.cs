using System.Threading.Tasks;
using SaveOnCloud.Core;
using LanguageExt;
using MediatR;
using Unit = LanguageExt.Unit;

namespace SaveOnCloud.Application.Courses.Commands
{
    public class UpdateCourse : Record<UpdateCourse>, IRequest<Validation<Error, Task<Unit>>>
    {
        public UpdateCourse(int courseId, string title, int credits, int departmentId)
        {
            CourseId = courseId;
            Title = title;
            Credits = credits;
            DepartmentId = departmentId;
        }

        public int CourseId { get; }
        public string Title { get; }
        public int Credits { get; }
        public int DepartmentId { get; }
    }
}
