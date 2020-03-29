using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARMClient.Authentication.AADAuthentication;
using ARMClient.Authentication.Contracts;
using ARMClient.Authentication.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Consumption;
using Microsoft.Azure.Management.Consumption.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Microsoft.Rest.Azure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SaveOnCloudApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IConfiguration _config;
        //Initialize all relevant objects
        static List<UsageDetail> usageDetails = new List<UsageDetail>();
        static Dictionary<string, List<UsageDetail>> usageDetailsByInstance = new Dictionary<string, List<UsageDetail>>();
        static List<PriceSheetProperties> priceSheet = new List<PriceSheetProperties>();
        static decimal? sumCost = new decimal?();

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //Login with ARM using ARMClient.Authorization dll
            TokenCredentials creds = GetTokenCredentialsWithARMClient();

            // Create the consumption client and obtain data for the configured subscription
            using (ConsumptionManagementClient consumptionClient = new ConsumptionManagementClient(creds))
            {
                consumptionClient.SubscriptionId = _config["SubscriptionId"];

                //Get usage details and perform some basic processing
                ProcessUsageDetails(consumptionClient);

                //Sum the cost of one of your resources based on usage detail cost info
                SumTotalCostOfAResource();

                //Get the full price sheet
                GetFullPriceSheet(consumptionClient);

                //Create a new sample budget
                CreateTestBudget(consumptionClient);
            }

            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// A method that queries ARM to obtain a user bearer token to use with the Consumption client.
        /// </summary>
        /// <returns>The token credentials for the user</returns>
        private TokenCredentials GetTokenCredentialsWithARMClient()
        {
            //Login with ARM using ARMClient.Authorization dll
            var persistentAuthHelper = new PersistentAuthHelper
            {
                AzureEnvironments = AzureEnvironments.Prod
            };
            TokenCacheInfo cacheInfo = null;
            persistentAuthHelper.AzureEnvironments = Utils.GetDefaultEnv();

            //Acquire tokens
            persistentAuthHelper.AcquireTokens().Wait();
            cacheInfo = persistentAuthHelper.GetToken(_config["SubscriptionId"], null).Result;
            TokenCredentials creds = new TokenCredentials(cacheInfo.AccessToken, "Bearer");

            return creds;
        }

        /// <summary>
        /// Method that queries and processes usage details.
        /// </summary>
        /// <param name="consumptionClient">The Consumption client.</param>
        private void ProcessUsageDetails(ConsumptionManagementClient consumptionClient)
        {
            //Grab the usage details for this month
            IPage<UsageDetail> usagePage = consumptionClient.UsageDetails.List("properties/meterDetails");
            ProcessUsagePage(usagePage);

            //Handle subsequent pages
            string nextPageLink = usagePage.NextPageLink;
            while (nextPageLink != null)
            {
                IPage<UsageDetail> nextUsagePage = consumptionClient.UsageDetails.ListNext(nextPageLink);
                ProcessUsagePage(nextUsagePage);
                nextPageLink = nextUsagePage.NextPageLink;
            }
        }

        /// <summary>
        /// Method that process usage pages and places them into a generic list and dictionary by instanceId
        /// </summary>
        /// <param name="usagePage">The usage page</param>
        private void ProcessUsagePage(IPage<UsageDetail> usagePage)
        {
            IEnumerator enumerator = usagePage.GetEnumerator();

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
        }

        /// <summary>
        /// Method that summarizes the cost of every usage detail record that pertains to one resource
        /// </summary>
        private void SumTotalCostOfAResource()
        {
            //Calculate the cost for the first usage detail resource in the general list
            sumCost = 0;
            foreach (UsageDetail info in usageDetailsByInstance[usageDetails.First().InstanceId])
            {
                sumCost += info.PretaxCost;
            }
        }

        /// <summary>
        /// Method that creates a test budget
        /// </summary>
        /// <param name="consumptionClient"></param>
        private void CreateTestBudget(ConsumptionManagementClient consumptionClient)
        {
            //Create a new budget object with the calculated cost above
            Budget newBudget = new Budget
            {
                Amount = (decimal)sumCost,
                Category = "Cost",
                Filters = new Filters
                {
                    Resources = new List<string>
                    {
                        usageDetails.First().InstanceId
                    }
                },
                TimeGrain = "Monthly",
                TimePeriod = new BudgetTimePeriod
                {
                    StartDate = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day + 1),
                    EndDate = DateTime.UtcNow.AddMonths(2)
                }

                /* Notifications for this budget can also be configured, uncomment the code below to add one
                ,Notifications = new Dictionary<string, Notification>()
                {
                    {
                        "NotificationName", new Notification
                        {
                            Enabled = true,
                            Threshold = (decimal)0.90,
                            ContactEmails = new List<string> { "YOUREMAIL@EMAIL.com"},
                            ContactGroups = null,
                            ContactRoles = null,
                            OperatorProperty = "GreaterThan"
                        }
                    }
                }
                */
            };

            Console.WriteLine("What would you like your budget to be named? Please type your response below: ");
            string budgetName = Console.ReadLine();

            //Create the budget
            consumptionClient.Budgets.CreateOrUpdate(
                budgetName,
                newBudget);
        }

        /// <summary>
        /// Method that queries to obtain the full price sheet.
        /// </summary>
        /// <param name="consumptionClient"></param>
        private static void GetFullPriceSheet(ConsumptionManagementClient consumptionClient)
        {
            //Get price first price sheet result and put the properties into a list
            PriceSheetResult priceSheetResult = consumptionClient.PriceSheet.Get();
            foreach (PriceSheetProperties properties in priceSheetResult.Pricesheets)
            {
                priceSheet.Add(properties);
            }

            //Process subsequest price sheet results
            while (priceSheetResult.NextLink != "")
            {
                Uri nextPriceSheetLink = new Uri(priceSheetResult.NextLink);
                var query = HttpUtility.ParseQueryString(nextPriceSheetLink.Query);
                string skipToken = query.Get("$skiptoken");

                priceSheetResult = consumptionClient.PriceSheet.Get(null, skipToken);
                foreach (PriceSheetProperties properties in priceSheetResult.Pricesheets)
                {
                    priceSheet.Add(properties);
                }
            }
        }
    }
}
