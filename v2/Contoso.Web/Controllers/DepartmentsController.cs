using System.Threading.Tasks;
using SaveOnCloud.Application.Departments.Commands;
using SaveOnCloud.Application.Departments.Queries;
using SaveOnCloud.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SaveOnCloud.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DepartmentsController(IMediator mediatr) => _mediator = mediatr;

        [HttpGet("{departmentId}")]
        public Task<IActionResult> Get(int departmentId) => 
            _mediator.Send(new GetDepartmentById(departmentId)).ToActionResult();

        [HttpPost]
        public Task<IActionResult> Create([FromBody] CreateDepartment createDepartment) =>
            _mediator.Send(createDepartment).ToActionResult();

        [HttpPut]
        public Task<IActionResult> Update([FromBody] UpdateDepartment updateDepartment) =>
            _mediator.Send(updateDepartment).ToActionResult();

        [HttpDelete]
        public Task<IActionResult> Delete([FromBody] DeleteDepartment deleteDepartment) =>
            _mediator.Send(deleteDepartment).ToActionResult();
    }
}
