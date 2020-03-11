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
        public void report(string organisation)
        {
            req = new APIRequest(Session["PAT"].ToString());
            string url;
            url = BaseURL + "/" + organisation + "/_apis/projects?api-version=" + version;
            
            string response = req.ApiRequest(url);
            org = JsonConvert.DeserializeObject<OrgModel>(response);
            countGen count = new countGen();
            org.counts = new orgCounts();
            foreach (var project in org.Value)
            {                
                url= BaseURL + organisation + "/"+project.Name+"/_apis/build/definitions?api-version=" + version;
                response = req.ApiRequest(url);
                count= JsonConvert.DeserializeObject<countGen>(response);
                org.counts.buildDefCount += count.Count;
                url= BaseURLvsrm + organisation + "/"+project.Name+ "/_apis/release/definitions?api-version=" + version;
                response = req.ApiRequest(url);
                count= JsonConvert.DeserializeObject<countGen>(response);
                org.counts.releaseDefCount += count.Count;
            }
            
        }
    }
}