using distribution_copy.BL;
using distribution_copy.Models.Model_AK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace distribution_copy.Controllers
{
    public class CopyTestController : Controller
    {
        // GET: CopyTest
        BussinessLogicCopy logic = new BussinessLogicCopy();
        TestPlan testData = new TestPlan();
        // GET: Test
        public ActionResult Index()
        {


            return View();
        }

        public ActionResult TestDisplay()
        {
            Org org123 = new Org();

            return View(datastored);
        }
        Org datastored = new Org();

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

        {
            TestSuit dataretrive = new TestSuit();

            dataretrive = logic.TestSuit(planid);
            ViewBag.data1234 = dataretrive;
            Session["responsedata"] = dataretrive;
            return Json(dataretrive, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DestCopyTest(List<jsonResp> testcase, string testPlan, string testSuit)
        {
            CreateTest t1 = new CreateTest();
            foreach (var item in testcase)
            {
                WorkItemsClass testcaseItem = logic.TestCaseRetrive(item.id);
                //getting the details of the testcase with the ID

                //after we get the details from of the id,we should create a new testcase
                TestCaseResponseModel testResponse = t1.TestCaseCreate(testcaseItem);
                if (testResponse != null)
                {
                    t1.AddtoTestCaseToTestSuit(testPlan, testSuit, testResponse.id.ToString());
                }

            }
            bool result = false;
            if (CheckValid.AddTestcase == true)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTestCasesFromSuits(string planId, string suitid)
        {
            TestCasesFromSuits dataretrive = new TestCasesFromSuits();
            dataretrive = logic.TestCaseFromSuit(planId, suitid);

            return Json(dataretrive, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CopyTestCase()
        {

            if (Session["PAT"] == null || Session["PAT"] == "")
            {
                return RedirectToAction("Index", "Account");

            }
            else
            {
                Org.pat = Session["PAT"].ToString();
            }

            return View();
        }
    }

    public class jsonResp
    {
        public string id { get; set; }
    }
}