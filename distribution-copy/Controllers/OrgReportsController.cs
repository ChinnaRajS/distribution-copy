using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using distribution_copy.Class;
using distribution_copy.Models;

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
        public void report(string organisation)
        {
            req = new APIRequest(Session["PAT"].ToString());
            
            string projectlist=APIRequest()
            
        }
    }
}