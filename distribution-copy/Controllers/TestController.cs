using distribution_copy.BL;
using distribution_copy.Models.Model_AK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace distribution_copy.Controllers
{
    public class TestController : Controller
    {
        BussinessLogic logic = new BussinessLogic();
        Org datastored = new Org();
        TestPlan testData = new TestPlan();
        // GET: Test
        public ActionResult Index()
        {
            var PAT = Session["PAT"].ToString();
          //  Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", token)));
            Org.pat = PAT;
            return View();
        }
        public JsonResult GetOrganization()
        {
            Profile profile = new Profile();

            Profile getProfileDetails = logic.profile();
            Organization getOrg = logic.Organization(getProfileDetails.id);
            return Json(getOrg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjects(string orgName)
        {
            Org.OrganizationName = orgName;
            datastored.ProjectNameList = logic.ProjectNamesStore();
            //TestDisplay();

            ViewBag.project = datastored;
            return Json(datastored.ProjectNameList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJsonPlans(string selectedProject)
        {
            Org.ProjectName = selectedProject;
            testData = logic.JsonRetrive();
            //datastored.ProjectNameList = logic.ProjectNamesStore();
            return Json(testData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTestSuits(string planid)
        // public ActionResult GetTestSuits(string planid)
        {
            TestSuit dataretrive = new TestSuit();

            dataretrive = logic.TestSuit(planid);
            //  ViewBag.datastore = dataretrive;
            ViewBag.data1234 = dataretrive;
            Session["responsedata"] = dataretrive;

            //     return View("TestDisplay");
            return Json(dataretrive, JsonRequestBehavior.AllowGet);
        }


    }
}