using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        string BaseURL = ConfigurationManager.AppSettings["BaseURL"];
        string BaseURLvsrm = ConfigurationManager.AppSettings["BaseURLvsrm"];
        string version = ConfigurationManager.AppSettings["ApiVersion"];
        public JsonResult report(string organisation)
        {
            if (organisation == "0")
                return Json(null);

            req = new APIRequest(Session["PAT"].ToString());
            string url;
            url = BaseURL + "/" + organisation + "/_apis/projects?api-version=" + version;            
            string response = req.ApiRequest(url);
            org = JsonConvert.DeserializeObject<OrgModel>(response);
            countGen count = new countGen();
            org.counts = new orgCounts();
            url = BaseURL + "/" + organisation + "/_apis/projects?api-version=" + version;
            response = req.ApiRequest(url);
            count = JsonConvert.DeserializeObject<countGen>(response);
            org.counts.processCount = count.Count;
            foreach (var project in org.Value)
            {                
                url= BaseURL + organisation + "/"+project.Name+"/_apis/build/definitions?api-version=" + version;
                response = req.ApiRequest(url);
                project.counts = new orgCounts();
                count = JsonConvert.DeserializeObject<countGen>(response);               
                project.counts.buildDefCount = count.Count;
                org.counts.buildDefCount += count.Count;
                url= BaseURLvsrm + organisation + "/"+project.Name+ "/_apis/release/definitions?api-version=" + version;
                response = req.ApiRequest(url);
                count= JsonConvert.DeserializeObject<countGen>(response);
                project.counts.releaseDefCount = count.Count;
                org.counts.releaseDefCount += count.Count;
            }
            
            return Json(org,JsonRequestBehavior.AllowGet);
        }
    }
}