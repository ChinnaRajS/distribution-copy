using distribution_copy.Models.ChartCountModel;
using distribution_copy.Models.ExpandWI;
using distribution_copy.Models.ProjectModel;
using distribution_copy.Models.ResponseWI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace distribution_copy.Controllers
{
    public class ChartController : Controller
    {
        public Services.AccountService service = new Services.AccountService();

        // GET: distribution
        public ActionResult Index()
        {
            if (Session["visited"] == null)
                return RedirectToAction("../Account/Verify");

            return View();
        }

        public JsonResult WITypesCount(string orgName)
        {
            Dictionary<string, Dictionary<string,int>> CountByProject = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> CountByOrg = new Dictionary<string,  int>();
            var pm = service.GetApi<ProjectModel>("https://dev.azure.com/" + orgName + "/_apis/projects?api-version=5.1");
            foreach (var project in pm.Value)
            {
                var response = service.GetApi<distribution_copy.Models.WorkItemType.WorkItemType>("https://dev.azure.com/" + orgName + "/" + project.Name + "/_apis/wit/workitemtypes?api-version=5.1");
                ResponseWI responseType=new ResponseWI();
                Dictionary<string, int> types = new Dictionary<string, int>();
                foreach (var TypeName in response.Value)
                {
                    string queryString = "Select [Id] From WorkItems Where [System.WorkItemType] = '" + TypeName.Name + "' And [System.TeamProject]='" + project.Name + "'";
                    ProjectModel model = new ProjectModel();
                    string url = "https://dev.azure.com/" + orgName + "/_apis/wit/wiql?api-version=5.1";
                    var wiql = new
                    {
                        query = queryString
                    };
                    var content = JsonConvert.SerializeObject(wiql);

                    responseType = service.GetApi<ResponseWI>(url, "POST", content);
                    if(responseType.workItems.Count!=0)
                        types.Add(TypeName.Name, responseType.workItems.Count);
                }
                CountByProject.Add(project.Name, types);
            }
            foreach(var proj in CountByProject.Keys)
            {
                foreach(var Type in CountByProject[proj].Keys)
                {
                    if (CountByOrg.ContainsKey(Type))
                    {
                        CountByOrg[Type] += CountByProject[proj][Type];
                    }
                    else
                    {
                        CountByOrg.Add(Type, CountByProject[proj][Type]);
                    }
                }
            }
            ChartCountModel chartCount = new ChartCountModel();
            chartCount.CountByOrg = CountByOrg;
            chartCount.CountByProject = CountByProject;
            return Json(chartCount);
        }
    }
}