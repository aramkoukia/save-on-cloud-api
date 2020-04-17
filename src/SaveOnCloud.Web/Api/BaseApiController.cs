using Microsoft.AspNetCore.Mvc;

namespace SaveOnCloud.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
    }
}
