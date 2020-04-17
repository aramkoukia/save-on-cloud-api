using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Management.Billing;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Billing.Models;

namespace SaveOnCloud.Web.Controllers.Azure
{
    [Route("azure/[controller]")]
    [ApiController]
    public class BillingController : Controller
    {
        private readonly IConfiguration Configuration;

        public BillingController(IConfiguration configuration) => Configuration = configuration;

        [HttpGet]
        public async Task<BillingAccountListResult> GetBilling()
        {
            var credentials = SdkContext.AzureCredentialsFactory
            .FromServicePrincipal(Configuration["clientId"], Configuration["clientSecret"], Configuration["tenantId"], AzureEnvironment.AzureGlobalCloud);

            var billingClient = new BillingManagementClient(credentials)
            {
                SubscriptionId = Configuration["SubscriptionId"]
            };

            // Get list of invoices
            // return await billingClient.BillingSubscriptions.ListByBillingAccount()
            return await billingClient.BillingAccounts.ListAsync();
        }
    }
}