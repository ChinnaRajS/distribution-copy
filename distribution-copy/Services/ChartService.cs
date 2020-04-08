using System;
using distribution_copy.Models.ChartCountModel;
using distribution_copy.Models.ProjectModel;
using distribution_copy.Models.ResponseWIAPI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace distribution_copy.Services
{
    public class ChartService
    {
        public Services.AccountService service = new Services.AccountService();

        public ChartCountModel ChartValues(string orgName)
        {
            ResponseWIAPI urlResponse = new ResponseWIAPI();
            string queryString = "Select [Id] From WorkItems";
            string url = "https://dev.azure.com/" + orgName + "/_apis/wit/wiql?api-version=5.1";
            var wiql = new
            {
                query = queryString
            };
            var content = JsonConvert.SerializeObject(wiql);

            distribution_copy.Models.ResponseWI.ResponseWI wiqlResponse = service.GetApi<distribution_copy.Models.ResponseWI.ResponseWI>(url, "POST", content);

            if (wiqlResponse == null || wiqlResponse.workItems.Count == 0)
                return null;
            string defaultUrl = "https://dev.azure.com/" + orgName + "/_apis/wit/workitems?ids=";
            url = defaultUrl;
            urlResponse.value = new List<Value>();
            string b = "&api-version=5.1";
            for (int j = 0; j < wiqlResponse.workItems.Count; j++)
            {
                if (j % 200 == 0 && j != 0)
                {

                    var batchResponse = service.GetApi<ResponseWIAPI>(url + b);
                    urlResponse.count += batchResponse.count;
                    foreach (var item in batchResponse.value)
                    {
                        urlResponse.value.Add(item);
                    }
                    url = defaultUrl;
                }
                if (j % 200 == 0)
                {
                    url += wiqlResponse.workItems[j].id;
                }
                else
                {
                    url += "," + wiqlResponse.workItems[j].id;
                }
            }
            url += b;

            var lastBatchResponse = service.GetApi<ResponseWIAPI>(url);
            urlResponse.count += lastBatchResponse.count;
            foreach (var item in lastBatchResponse.value)
                urlResponse.value.Add(item);

            Dictionary<string, Dictionary<string, int>> CountByProject = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> CountByOrg = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, int>> BugChart = new Dictionary<string, Dictionary<string, int>>();

            foreach (var Wi in urlResponse.value)
            {
                if (!CountByOrg.ContainsKey(Wi.fields.WorkItemType))
                    CountByOrg.Add(Wi.fields.WorkItemType, 1);
                else
                    CountByOrg[Wi.fields.WorkItemType] += 1;

                if (CountByProject.ContainsKey(Wi.fields.TeamProject))
                {

                    if (!CountByProject[Wi.fields.TeamProject].ContainsKey(Wi.fields.WorkItemType))
                        CountByProject[Wi.fields.TeamProject].Add(Wi.fields.WorkItemType, 1);
                    else
                        CountByProject[Wi.fields.TeamProject][Wi.fields.WorkItemType] += 1;
                }
                else
                    CountByProject.Add(Wi.fields.TeamProject, new Dictionary<string, int>() { { Wi.fields.WorkItemType, 1 } });

                if (Wi.fields.WorkItemType.ToLower() == "bug")
                {
                    if (!BugChart.ContainsKey(Wi.fields.TeamProject))
                        BugChart.Add(Wi.fields.TeamProject, new Dictionary<string, int>() { { Wi.fields.Severity, 1 } });
                    else
                    {
                        if (BugChart[Wi.fields.TeamProject].ContainsKey(Wi.fields.Severity))
                        {
                            BugChart[Wi.fields.TeamProject][Wi.fields.Severity] += 1;
                        }
                        else
                        {
                            BugChart[Wi.fields.TeamProject].Add(Wi.fields.Severity, 1);
                        }
                    }

                }


            }

            ChartCountModel chartCount = new ChartCountModel
            {
                CountByOrg = CountByOrg,
                CountByProject = CountByProject,
                BugChart = BugChart
            };
            return chartCount;
        }
    }
}