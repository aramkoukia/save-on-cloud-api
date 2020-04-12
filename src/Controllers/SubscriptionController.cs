using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaveOnCloudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync(object model)
        {
            return await Task.FromResult<IActionResult>(Ok());

        }
    }
}