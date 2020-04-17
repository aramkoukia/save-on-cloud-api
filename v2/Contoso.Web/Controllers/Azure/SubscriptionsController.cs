using System.Threading.Tasks;
using SaveOnCloud.Application.Courses.Commands;
using SaveOnCloud.Application.Courses.Queries;
using SaveOnCloud.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SaveOnCloud.Web.Controllers.Azure
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionsController(IMediator mediator) => _mediator = mediator;

        [HttpGet()]
        public Task<IActionResult> Get(int courseId) =>
            _mediator.Send(new GetCourseById(courseId)).ToActionResult();

        [HttpPost]
        public Task<IActionResult> Create(CreateCourse createCourse) =>
            _mediator.Send(createCourse).ToActionResult();
        
        [HttpPut]
        public Task<IActionResult> Update(UpdateCourse updateCourse) =>
            _mediator.Send(updateCourse).ToActionResult();

        [HttpDelete]
        public Task<IActionResult> Delete(DeleteCourse deleteCourse) =>
            _mediator.Send(deleteCourse).ToActionResult();

        [HttpGet("courseassignments/{courseId}")]
        public async Task<IActionResult> GetCourseAssignments(int courseId) => 
            Ok(await _mediator.Send(new GetCourseAssignments(courseId)));

        [HttpPost("courseassignments")]
        public Task<IActionResult> CreateCourseAssignment(CreateCourseAssignment createCourseAssignment) =>
            _mediator.Send(createCourseAssignment).ToActionResult();
    }
}
