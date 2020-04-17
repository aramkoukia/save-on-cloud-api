using System;
using System.Collections.Generic;
using System.Text;
using SaveOnCloud.Core.Domain;
using LanguageExt;
using MediatR;

namespace SaveOnCloud.Application.Courses.Queries
{
    public class GetCourseAssignments : Record<GetCourseAssignments>, IRequest<List<CourseAssignmentViewModel>>
    {
        public GetCourseAssignments(int courseId) => CourseId = courseId;
        public int CourseId { get; }
    }
}
