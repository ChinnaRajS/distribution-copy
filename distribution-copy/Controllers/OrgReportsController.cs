﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using distribution_copy.Class;
using distribution_copy.Models.InputModel;
using distribution_copy.Models.OrgModel;
using distribution_copy.Models.ProjectModel;
using distribution_copy.Models.WorkItemType;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace distribution_copy.Controllers
{
    public class OrgReportsController : Controller
    {
        // GET: OrgReports
        public ActionResult Index()
        {
            if (Session["visited"] == null)
            {
                return RedirectToAction("../Account/Verify");
            }
            return View();
        }
        APIRequest req;
        OrgModel org = new OrgModel();
        orgCounts c = new orgCounts();
        public Services.AccountService service = new Services.AccountService();

        string BaseURL = ConfigurationManager.AppSettings["BaseURL"];
        string BaseURLvsrm = ConfigurationManager.AppSettings["BaseURLvsrm"];
        string version = ConfigurationManager.AppSettings["ApiVersion"];
        string version1 = "5.1-preview";
        public JsonResult report(string organisation, string workitemtype = "0", string projectName = "0")
        {
            if (workitemtype != "0")
            {
                c.WIcountType = GetWorkitemCountByType(organisation, workitemtype, projectName);
                org.counts = c;
                return Json(org, JsonRequestBehavior.AllowGet);
            }
            req = new APIRequest(Session["PAT"].ToString());
            string url;
            url = BaseURL + "/" + organisation + "/_apis/projects?api-version=" + version;
            string response = req.ApiRequest(url);
            org = JsonConvert.DeserializeObject<OrgModel>(response);
            countGen count = new countGen();
            org.counts = new orgCounts();
            url = BaseURL + "/" + organisation + "/_apis/process/processes?api-version=" + version;
            response = req.ApiRequest(url);
            count = JsonConvert.DeserializeObject<countGen>(response);
            c.processCount = count.Count;
            foreach (var project in org.Value)
            {
                url = BaseURL + organisation + "/" + project.Name + "/_apis/build/definitions?api-version=" + version;
                response = req.ApiRequest(url);
                project.counts = new orgCounts();
                count = JsonConvert.DeserializeObject<countGen>(response);
                project.counts.buildDefCount = count.Count;
                c.buildDefCount += count.Count;
                url = BaseURLvsrm + organisation + "/" + project.Name + "/_apis/release/definitions?api-version=" + version;
                response = req.ApiRequest(url);
                count = JsonConvert.DeserializeObject<countGen>(response);
                project.counts.releaseDefCount = count.Count;
                c.releaseDefCount += count.Count;
                url = BaseURL + organisation + "/" + project.Name + "/_apis/git/repositories?api-version=" + version;
                response = req.ApiRequest(url);
                count = JsonConvert.DeserializeObject<countGen>(response);
                project.counts.repoCount = count.Count;
                //c.repoCount += count.Count;

            }

            // Calling Repos Count
            AllReposCount(organisation);
            // Calling Users Count
            AllUsersCount(organisation);
            // Calling WorkitemsCount Count
            AllWorKitemsCount(organisation);
            WITypes(organisation);
            //Calling WorkItemCountByType
            org.counts = c;
            return Json(org, JsonRequestBehavior.AllowGet);
        }

        //Author:Ravivarma (10/03/2020) -To get the count of all repositories in the organisation
        [HttpPost]
        public void AllReposCount(string organisation)
        {
            APIRequest req;
            //OrgModel org = new OrgModel();
            string url;
            try
            {
                url = BaseURL + "/" + organisation + "/_apis/git/repositories?api-version=" + version;
                req = new APIRequest(Session["PAT"].ToString());
                string response = req.ApiRequest(url);
                countGen count = JsonConvert.DeserializeObject<countGen>(response);
                c.repoCount = count.Count;
            }
            catch (Exception)
            {

            }

            //return org;
        }

        //Author:Ravivarma (11/03/2020) -To get the count of all users in the organisation
        [HttpPost]
        public void AllUsersCount(string organisation)
        {
            bool added = false;
            HttpClient client = new HttpClient();
            ProjectModel projModel = new ProjectModel();
            req = new APIRequest(Session["PAT"].ToString());
            string url;
            string response;
            /*   url = BaseURL + "/" + organisation + "/_apis/projects?api-version=" + version1;
               string response = req.ApiRequest(url);
               org = JsonConvert.DeserializeObject<OrgModel>(response);*/
            //org.Value = projModel.Value;
            //org.counts = new orgCounts();
            List<string> MemberCount;
            foreach (var projeId in org.Value)
            {
                url = "https://vssps.dev.azure.com/" + organisation + "/_apis/graph/descriptors/" + projeId.Id + "?api-version=" + version1 + ".1";
                response = req.ApiRequest(url);
                JObject jobj = JObject.Parse(response);


                url = "https://vssps.dev.azure.com/" + organisation + "/_apis/graph/groups?scopeDescriptor=" + jobj["value"] + "&api-version=" + version1;
                response = req.ApiRequest(url);
                ProjectModel Grp = JsonConvert.DeserializeObject<ProjectModel>(response);
                //projeId.counts = new orgCounts();

                MemberCount = new List<string>();
                foreach (var group in Grp.Value)
                {
                    url = "https://vsaex.dev.azure.com/" + organisation + "/_apis/GroupEntitlements/" + group.originId + "/members?api-version=" + version1 + ".1";
                    response = req.ApiRequest(url);
                    MembersMod Model = JsonConvert.DeserializeObject<MembersMod>(response);

                    foreach (var mem in Model.members)
                    {
                        if (!MemberCount.Contains(mem.id))
                        {
                            added = true;
                            MemberCount.Add(mem.id);

                        }
                        if (added == true)
                        {
                            projeId.counts.UserCount = MemberCount.Count;

                            added = false;
                        }
                    }



                }

                //org.counts.UserCount = MemberCount.Count;
                c.UserCount = MemberCount.Count;



            }

            //return org;
        }

        //Author:Ravivarma (11/03/2020) -To get the count of all Workitems in the organisation
        [HttpPost]
        public void AllWorKitemsCount(string organisation)
        {
            APIRequest req;
            OrgModel org = new OrgModel();
            string url;
            try
            {
                object Wiql = new { query = "Select  [Id] From WorkItems" };
                ProjectModel model = new ProjectModel();
                url = "https://dev.azure.com/" + organisation + "/_apis/wit/wiql?api-version=5.1";
                req = new APIRequest(Session["PAT"].ToString());
                string response = req.ApiRequest(url, "POST", JsonConvert.SerializeObject(Wiql));
                model = JsonConvert.DeserializeObject<ProjectModel>(response);
                c.WIcountOrg = model.WorkItems.Count;
            }
            catch (Exception)
            {

            }

            //return org;
        }

        //Author:Ravivarma (12/03/2020) -To get the count of all Workitems by types in the organisation

        public int GetWorkitemCountByType(string organisation, string workitemtype, string projectName)
        {

            APIRequest req;
            string url;
            try
            {
                string queryString = "Select  [Id] From WorkItems Where [System.WorkItemType] = '" + workitemtype + "' ";
                object Wiql = new { query = queryString };
                ProjectModel model = new ProjectModel();
                url = "https://dev.azure.com/" + organisation + "/_apis/wit/wiql?api-version=5.1";
                req = new APIRequest(Session["PAT"].ToString());
                string response = req.ApiRequest(url, "POST", JsonConvert.SerializeObject(Wiql));
                model = JsonConvert.DeserializeObject<ProjectModel>(response);
                c.WIcountType = model.WorkItems.Count;
                if (projectName != "0")
                {
                    queryString = "Select  [Id] From WorkItems Where [System.WorkItemType] = '" + workitemtype + "' AND   [System.TeamProject] ='" + projectName + "'";
                    Wiql = new { query = queryString };
                    url = "https://dev.azure.com/" + organisation + "/_apis/wit/wiql?api-version=5.1";
                    response = req.ApiRequest(url, "POST", JsonConvert.SerializeObject(Wiql));
                    model = JsonConvert.DeserializeObject<ProjectModel>(response);
                    c.ProjWIcountByType = model.WorkItems.Count;
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return c.WIcountType;
        }
        public void WITypes(string orgName)
        {
            List<string> WorkItemTypes = new List<string>();
            var pm = service.GetApi<ProjectModel>("https://dev.azure.com/" + orgName + "/_apis/projects?api-version=5.1");
            foreach (var project in pm.Value)
            {
                var response = service.GetApi<WorkItemType>("https://dev.azure.com/" + orgName + "/" + project.Name + "/_apis/wit/workitemtypes?api-version=5.1");
                foreach (var TypeName in response.Value)
                {
                    if (!(WorkItemTypes.Contains(TypeName.Name)))
                    {
                        WorkItemTypes.Add(TypeName.Name);
                    }
                }
            }
            org.WiTypes = WorkItemTypes;
        }
    }
}