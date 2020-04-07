using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Consumption;
using Microsoft.Azure.Management.Consumption.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Newtonsoft.Json;

namespace SaveOnCloudApi.Controllers.Azure
{
    [Route("azure/[controller]")]
    public class CostController : Controller
    {
        private readonly IConfiguration Configuration;

        public CostController(IConfiguration configuration) => Configuration = configuration;

        [HttpGet]
        public IEnumerable<LabelValueModel> GetCost()
        {
            var usageDetail = GetUsageDetail();
            return usageDetail.SelectMany(kvp => kvp.Value).Distinct()
                .GroupBy(l => l.InstanceName)
                .Select(cl => new LabelValueModel
                {
                    label = cl.First().InstanceName,
                    value = cl.Sum(c => c.PretaxCost).ToString(),
                })
                .OrderBy(o => o.value);
        }

        [HttpGet("daily")]
        public IEnumerable<DateValueModel> GetDailySpending()
        {
            var usageDetail = GetUsageDetail();
            return usageDetail.SelectMany(kvp => kvp.Value).Distinct()
                .GroupBy(l => l.UsageStart.Value.ToShortDateString())
                .Select(cl => new DateValueModel
                {
                    date = cl.First().UsageStart.Value.ToShortDateString(),
                    value = cl.Sum(c => c.PretaxCost).ToString(),
                })
                .OrderBy(o => o.date);
        }

        [HttpGet("dayoverday")]
        public IEnumerable<DateValueModel> GetDayOverDaySpending()
        {
            var usageDetail = GetUsageDetail();
            return usageDetail.SelectMany(kvp => kvp.Value).Distinct()
                .GroupBy(l => l.UsageStart.Value.ToShortDateString())
                .Select(cl => new DateValueModel
                {
                    date = cl.First().UsageStart.Value.ToShortDateString(),
                    value = cl.Sum(c => c.PretaxCost).ToString(),
                })
                .OrderBy(o => o.date);
        }

        [HttpGet("detail")]
        public string GetDetail() => JsonConvert.SerializeObject(GetUsageDetail());

        private Dictionary<string, List<UsageDetail>> GetUsageDetail()
        {
            // Create the consumption client and obtain data for the configured subscription
            var credentials = SdkContext.AzureCredentialsFactory
                        .FromServicePrincipal(Configuration["clientId"], Configuration["clientSecret"], Configuration["tenantId"], AzureEnvironment.AzureGlobalCloud);

            using ConsumptionManagementClient consumptionClient = new ConsumptionManagementClient(credentials)
            {
                SubscriptionId = Configuration["SubscriptionId"]
            };
            return ProcessUsageDetails(consumptionClient);
        }

        private Dictionary<string, List<UsageDetail>> ProcessUsageDetails(ConsumptionManagementClient consumptionClient)
        {
            //Grab the usage details for this month
            IPage<UsageDetail> usagePage = consumptionClient.UsageDetails.List("properties/meterDetails");
            var resource = ProcessUsagePage(usagePage);
            var result = new Dictionary<string, List<UsageDetail>>(resource);
            
            string nextPageLink = usagePage.NextPageLink;
            while (nextPageLink != null)
            {
                IPage<UsageDetail> nextUsagePage = consumptionClient.UsageDetails.ListNext(nextPageLink);
                var resourceDictionary = ProcessUsagePage(nextUsagePage);
                result = result.Union(resourceDictionary).ToDictionary(d => d.Key, d => d.Value);
                nextPageLink = nextUsagePage.NextPageLink;
            }
            return result;
        }

        private Dictionary<string, List<UsageDetail>> ProcessUsagePage(IPage<UsageDetail> usagePage)
        {
            IEnumerator enumerator = usagePage.GetEnumerator();
            var usageDetails = new List<UsageDetail>();
            var usageDetailsByInstance = new Dictionary<string, List<UsageDetail>>();
            while (enumerator.MoveNext())
            {
                UsageDetail currentDetail = (UsageDetail)enumerator.Current;

                //Add the usage detail to the list
                usageDetails.Add(currentDetail);

                //Add the usage detail to the dictionary
                if (usageDetailsByInstance.ContainsKey(currentDetail.InstanceId))
                {
                    usageDetailsByInstance[currentDetail.InstanceId].Add(currentDetail);
                }
                else
                {
                    List<UsageDetail> usageDetailsListForResource = new List<UsageDetail>();
                    usageDetailsListForResource.Add(currentDetail);
                    usageDetailsByInstance.Add(currentDetail.InstanceId, usageDetailsListForResource);
                }
            }
            return usageDetailsByInstance;
        }
    }
}
