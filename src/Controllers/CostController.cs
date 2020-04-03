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

namespace SaveOnCloudApi.Controllers
{
    [Route("azure/[controller]")]
    public class CostController : Controller
    {
        private readonly IConfiguration Configuration;

        public CostController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<LabelValueModel> GetCost()
        {
            // Create the consumption client and obtain data for the configured subscription
            var credentials = SdkContext.AzureCredentialsFactory
                        .FromServicePrincipal(Configuration["clientId"], Configuration["clientSecret"], Configuration["tenantId"], AzureEnvironment.AzureGlobalCloud);

            using ConsumptionManagementClient consumptionClient = new ConsumptionManagementClient(credentials)
            {
                SubscriptionId = Configuration["SubscriptionId"]
            };

            //Get usage details and perform some basic processing
            var result = ProcessUsageDetails(consumptionClient);
            var data = result.SelectMany(kvp => kvp.Value).Distinct()
                .GroupBy(l => l.InstanceName)
                .Select(cl => new LabelValueModel
                {
                    label = cl.First().InstanceName,
                    value = cl.Sum(c => c.PretaxCost).ToString(),
                })
                .OrderBy(o => o.value);

            return data;
        }

        [HttpGet("daily")]
        public IEnumerable<DateValueModel> GetDailySpending()
        {
            // Create the consumption client and obtain data for the configured subscription
            var credentials = SdkContext.AzureCredentialsFactory
                        .FromServicePrincipal(Configuration["clientId"], Configuration["clientSecret"], Configuration["tenantId"], AzureEnvironment.AzureGlobalCloud);

            using ConsumptionManagementClient consumptionClient = new ConsumptionManagementClient(credentials)
            {
                SubscriptionId = Configuration["SubscriptionId"]
            };

            //Get usage details and perform some basic processing
            var result = ProcessUsageDetails(consumptionClient);
            var data = result.SelectMany(kvp => kvp.Value).Distinct()
                .GroupBy(l => l.UsageStart.Value.ToShortDateString())
                .Select(cl => new DateValueModel
                {
                    date = cl.First().UsageStart.Value.ToShortDateString(),
                    value = cl.Sum(c => c.PretaxCost).ToString(),
                })
                .OrderBy(o => o.date);

            return data;
        }

        [HttpGet("detail")]
        public string GetDetail()
        {
            // Create the consumption client and obtain data for the configured subscription
            var credentials = SdkContext.AzureCredentialsFactory
                        .FromServicePrincipal(Configuration["clientId"], Configuration["clientSecret"], Configuration["tenantId"], AzureEnvironment.AzureGlobalCloud);

            using ConsumptionManagementClient consumptionClient = new ConsumptionManagementClient(credentials)
            {
                SubscriptionId = Configuration["SubscriptionId"]
            };

            //Get usage details and perform some basic processing
            var result = ProcessUsageDetails(consumptionClient);
            // GetFullPriceSheet(consumptionClient);

            //Sum the cost of one of your resources based on usage detail cost info
            // SumTotalCostOfAResource();

            //Get the full price sheet
            // GetFullPriceSheet(consumptionClient);

            //Create a new sample budget
            // CreateTestBudget(consumptionClient);
            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// Method that queries and processes usage details.
        /// </summary>
        /// <param name="consumptionClient">The Consumption client.</param>
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

        /// <summary>
        /// Method that process usage pages and places them into a generic list and dictionary by instanceId
        /// </summary>
        /// <param name="usagePage">The usage page</param>
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

        /// <summary>
        /// Method that summarizes the cost of every usage detail record that pertains to one resource
        /// </summary>
        //private void SumTotalCostOfAResource()
        //{
        //    //Calculate the cost for the first usage detail resource in the general list
        //    var sumCost = 0;
        //    foreach (UsageDetail info in usageDetailsByInstance[usageDetails.First().InstanceId])
        //    {
        //        sumCost += info.PretaxCost;
        //    }
        //}

        /// <summary>
        /// Method that creates a test budget
        /// </summary>
        /// <param name="consumptionClient"></param>
        //private void CreateTestBudget(ConsumptionManagementClient consumptionClient)
        //{
        //    //Create a new budget object with the calculated cost above
        //    Budget newBudget = new Budget
        //    {
        //        Amount = (decimal)sumCost,
        //        Category = "Cost",
        //        Filters = new Filters
        //        {
        //            Resources = new List<string>
        //            {
        //                usageDetails.First().InstanceId
        //            }
        //        },
        //        TimeGrain = "Monthly",
        //        TimePeriod = new BudgetTimePeriod
        //        {
        //            StartDate = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day + 1),
        //            EndDate = DateTime.UtcNow.AddMonths(2)
        //        }

        //        /* Notifications for this budget can also be configured, uncomment the code below to add one
        //        ,Notifications = new Dictionary<string, Notification>()
        //        {
        //            {
        //                "NotificationName", new Notification
        //                {
        //                    Enabled = true,
        //                    Threshold = (decimal)0.90,
        //                    ContactEmails = new List<string> { "YOUREMAIL@EMAIL.com"},
        //                    ContactGroups = null,
        //                    ContactRoles = null,
        //                    OperatorProperty = "GreaterThan"
        //                }
        //            }
        //        }
        //        */
        //    };

        //    //Create the budget
        //    consumptionClient.Budgets.CreateOrUpdate(
        //        budgetName,
        //        newBudget);
        //}

        /// <summary>
        /// Method that queries to obtain the full price sheet.
        /// </summary>
        /// <param name="consumptionClient"></param>
        //private static void GetFullPriceSheet(ConsumptionManagementClient consumptionClient)
        //{
        //    var priceSheet = new List<PriceSheetProperties>();
        //    //Get price first price sheet result and put the properties into a list
        //    PriceSheetResult priceSheetResult = consumptionClient.PriceSheet.Get();
        //    foreach (PriceSheetProperties properties in priceSheetResult.Pricesheets)
        //    {
        //        priceSheet.Add(properties);
        //    }

        //    //Process subsequest price sheet results
        //    while (priceSheetResult.NextLink != "")
        //    {
        //        Uri nextPriceSheetLink = new Uri(priceSheetResult.NextLink);
        //        var query = HttpUtility.ParseQueryString(nextPriceSheetLink.Query);
        //        string skipToken = query.Get("$skiptoken");

        //        priceSheetResult = consumptionClient.PriceSheet.Get(null, skipToken);
        //        foreach (PriceSheetProperties properties in priceSheetResult.Pricesheets)
        //        {
        //            priceSheet.Add(properties);
        //        }
        //    }
        //}
    }
}
