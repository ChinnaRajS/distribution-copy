using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using distribution_copy.Class;
using distribution_copy.Models;
using Newtonsoft.Json;

namespace distribution_copy.Controllers
{
    public class OrgReportsController : Controller
    {
        // GET: OrgReports
        public ActionResult Index()
        {
            return View();
        }
        APIRequest req;
        OrgModel org=new OrgModel();


        //Author:VidyaSagar
        public void report(string organisation)
        {
            req = new APIRequest(Session["PAT"].ToString());

            // string projectlist = APIRequest();



            //Calling a Method by passing Oraganization
            AllProjectCount(organisation);

        }




        //Author:Ravivarma (10/03/2020) -To get the count of all projects in the organisation
        [HttpPost]
        public JsonResult AllProjectCount(string Organization)
        {
            string responseBody = "";
            ProjectModel pm = new ProjectModel();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString()))));

                    using (HttpResponseMessage response = client.GetAsync("https://dev.azure.com/" + Organization + "/_apis/projects?api-version=5.1").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        responseBody = response.Content.ReadAsStringAsync().Result;
                    }

                }
                pm = JsonConvert.DeserializeObject<ProjectModel>(responseBody);
            }
            catch (Exception ex)
            {
                return Json("");
            }

            return Json(pm.Count, JsonRequestBehavior.AllowGet);
        }
    }
}