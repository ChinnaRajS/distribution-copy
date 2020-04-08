using distribution_copy.Models.ChartCountModel;
using distribution_copy.Models.ProjectModel;
using distribution_copy.Models.ResponseWIAPI;
using distribution_copy.Models.AccessDetails;
using distribution_copy.Models.AccountsResponse;
using distribution_copy.Models.ProfileDetails;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace distribution_copy.Controllers
{
    public class ChartController : Controller
    {
        public Services.AccountService service = new Services.AccountService();
        public Services.ChartService ChartService = new Services.ChartService();

        // GET: distribution
        public ActionResult Index()
        {
            if (Session["visited"] == null)
                return RedirectToAction("../Account/Verify");

            if (Session["PAT"] == null)
            {
                try
                {
                    AccessDetails _accessDetails = new AccessDetails();
                    AccountsResponse.AccountList accountList = null;
                    string code = Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString();
                    string redirectUrl = ConfigurationManager.AppSettings["RedirectUri"];
                    string clientId = ConfigurationManager.AppSettings["ClientSecret"];
                    string accessRequestBody = string.Empty;
                    accessRequestBody = service.GenerateRequestPostData(clientId, code, redirectUrl);
                    _accessDetails = service.GetAccessToken(accessRequestBody);
                    ProfileDetails profile = service.GetProfile(_accessDetails);
                    if (!string.IsNullOrEmpty(_accessDetails.access_token))
                    {
                        Session["PAT"] = _accessDetails.access_token;

                        if (profile.displayName != null || profile.emailAddress != null)
                        {
                            Session["User"] = profile.displayName ?? string.Empty;
                            Session["Email"] = profile.emailAddress ?? profile.displayName.ToLower();
                        }
                    }
                    accountList = service.GetAccounts(profile.id, _accessDetails);
                    Session["AccountList"] = accountList;
                    string pat = Session["PAT"].ToString();
                    List<SelectListItem> OrganizationList = new List<SelectListItem>();
                    foreach (var i in accountList.value)
                    {
                        OrganizationList.Add(new SelectListItem { Text = i.accountName, Value = i.accountName });
                    }
                    ViewBag.OrganizationList = OrganizationList;
                }
                catch (Exception) { }
            }
                return View();
        }

        public JsonResult WITypes(string orgName)
        {
            Dictionary<string, Dictionary<string, int>> CountByProject = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> CountByOrg = new Dictionary<string, int>();
            var pm = service.GetApi<ProjectModel>("https://dev.azure.com/" + orgName + "/_apis/projects?api-version=5.1");
            foreach (var project in pm.Value)
            {
                var response = service.GetApi<Models.WorkItemType.WorkItemType>("https://dev.azure.com/" + orgName + "/" + project.Name + "/_apis/wit/workitemtypes?api-version=5.1");
                Dictionary<string, int> types = new Dictionary<string, int>();
                foreach (var TypeName in response.Value)
                {
                    string queryString = "Select [Id] From WorkItems Where [System.WorkItemType] = '" + TypeName.Name + "' And [System.TeamProject]='" + project.Name + "'";
                    string url = "https://dev.azure.com/" + orgName + "/_apis/wit/wiql?api-version=5.1";
                    var wiql = new
                    {
                        query = queryString
                    };
                    var content = JsonConvert.SerializeObject(wiql);

                    ResponseWIAPI responseType = service.GetApi<ResponseWIAPI>(url, "POST", content);
                    if (responseType!=null&&responseType.value.Count != 0)
                        types.Add(TypeName.Name, responseType.value.Count);
                }
                CountByProject.Add(project.Name, types);
            }
            foreach (var proj in CountByProject.Keys)
            {
                foreach (var Type in CountByProject[proj].Keys)
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
            ChartCountModel chartCount = new ChartCountModel
            {
                CountByOrg = CountByOrg,
                CountByProject = CountByProject
            };
            return Json(chartCount);
        }
        public JsonResult WITypesCount(string orgName)
        {            
            return Json(ChartService.ChartValues(orgName));
        }
    }
}