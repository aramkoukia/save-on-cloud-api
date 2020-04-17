using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace SaveOnCloud.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public PaymentController(IConfiguration configuration) => Configuration = configuration;

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync(dynamic intent)
        {
            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = Configuration["stripApiKey"];

            var customerOptions = new CustomerCreateOptions
            {
                Email = "jenny.rosen@example.com",
                PaymentMethod = intent.PaymentMethodId,
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = intent.PaymentMethodId,
                },
            };

            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions);

            var items = new List<SubscriptionItemOptions> {
                new SubscriptionItemOptions {
                    Plan = "plan_CBXbz9i7AIOTzr"
                }
            };
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customer.Id,
                Items = items
            };
            subscriptionOptions.AddExpand("latest_invoice.payment_intent");

            var subscriptionService = new SubscriptionService();
            var subscription = subscriptionService.Create(subscriptionOptions);

            return await Task.FromResult<IActionResult>(Created("api/subscription", subscription));
        }
    }
}