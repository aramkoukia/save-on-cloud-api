using System.Threading.Tasks;
using SaveOnCloud.Application.Enrollments.Commands;
using SaveOnCloud.Application.Enrollments.Queries;
using SaveOnCloud.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SaveOnCloud.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EnrollmentsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{enrollmentId}")]
        public Task<IActionResult> Get(int enrollmentId) => 
            _mediator.Send(new GetEnrollmentById(enrollmentId)).ToActionResult();
        
        [HttpPost]
        public Task<IActionResult> Create(CreateEnrollment create) =>
            _mediator.Send(create).ToActionResult();

        [HttpPut]
        public Task<IActionResult> Update(UpdateEnrollment update) =>
            _mediator.Send(update).ToActionResult();

        [HttpDelete]
        public Task<IActionResult> Delete(DeleteEnrollment delete) =>
            _mediator.Send(delete).ToActionResult();
    }
}
