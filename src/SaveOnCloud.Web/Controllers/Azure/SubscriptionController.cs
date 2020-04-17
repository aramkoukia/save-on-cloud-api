using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaveOnCloud.Web.Models;
using SaveOnCloud.Web.Models.Azure;

namespace SaveOnCloud.Web.Controllers.Azure
{

    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger _logger;

        public PaymentController(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<PaymentController>();
        }

        [HttpPost]
        public IActionResult Post(CreateSubscriptionModel model)
        {
            _logger.LogInformation("Azure Subscription Added.");
            return Created("", model.SubscriptionId);
        }
    }
}