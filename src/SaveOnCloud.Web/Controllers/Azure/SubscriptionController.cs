using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SaveOnCloud.Core.Entities;
using SaveOnCloud.SharedKernel.Interfaces;
using SaveOnCloud.Web.Models;
using SaveOnCloud.Web.Models.Azure;

namespace SaveOnCloud.Web.Controllers.Azure
{

    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public PaymentController(IRepository repository,
                                 ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<PaymentController>();
        }

        [HttpPost]
        public IActionResult Post(CreateSubscriptionModel model)
        {
            _repository.Add(new AzureAccount
            {
                ClientId = model.SubscriptionId,
                CompanyId = 0,
                ClientSecret = model.ClientSecret,
                SubscriptionId = model.SubscriptionId,
                TenantId = model.TenantId
            });
            _logger.LogInformation("Azure Subscription Added.");
            return Created("", model.SubscriptionId);
        }
    }
}