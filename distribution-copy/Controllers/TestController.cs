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
            try
            {
                var PAT = Session["PAT"].ToString();
                //Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", token)));
                Org.pat = PAT;
            }
            catch
            {
                return RedirectToAction("Verify","Account");
            }

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

        TestResult ResultObj = new TestResult();
       
        //  public List<TestRunById> RunList = new List<TestRunById>();


        public JsonResult GetTestSuits(string planid)
        {
            List<TestResult> TestList = new List<TestResult>();
            ListClass listRun = new ListClass();
            TestResult ResultObjListStore = new TestResult();
            try
            {
                List<TestRunById> RunList = new List<TestRunById>();
                ResultObj.PassCount = 0;
                ResultObj.FailCount = 0;
                ResultObj.TestCaseCount = 0;

                TestRunProject testProData = logic.GetRunByProject();
                foreach (var test in testProData.value)
                {
                    TestRunById runData = logic.GetRunByRunId(test.id.ToString());
                    RunList.Add(runData);
                    ListClass.RunList.Add(runData);
                }
                //project level run
                TestSuit dataretrive = new TestSuit();

                dataretrive = logic.TestSuit(planid);
                foreach (var inf in dataretrive.value)
                {
                    TestCasesFromSuits testresult = logic.TestCaseFromSuit(inf.plan.id.ToString(), inf.id.ToString());
                    foreach (var test in testresult.value)
                    {
                        List<int> maxStore = new List<int>();
                        // List<string> runStore = new List<string>();
                        foreach (var run in RunList)
                        {

                            foreach (var run1 in run.value)
                            {
                                if (run1.testCase.id == null)
                                {
                                    if (run1.testCase.id.ToString() == test.workItem.id.ToString())
                                    {
                                        int runid = Convert.ToInt32(run1.testRun.id);
                                        maxStore.Add(runid);
                                    }
                                }
                            }
                        }

                        if (maxStore != null && maxStore.Count > 0)
                        {
                            var maxId = maxStore.Max();
                            //RunList.Find();
                            // TestRunById dataQuery =new TestRunById();
                            string outcome1 = string.Empty;
                            foreach (var i in RunList)
                            {
                                //  var dataQuery = i.value.Find(x => x.testRun.id == maxId.ToString());
                                foreach (var k in i.value)
                                {
                                    if (k.testRun.id == maxId.ToString())
                                    {
                                        outcome1 = k.outcome.ToString();
                                    }
                                }
                            }
                            string outcome = outcome1.ToString();
                            if (outcome == "Passed")
                            {
                                ResultObj.PassCount = ResultObj.PassCount + 1;
                            }
                            else if (outcome == "Failed")
                            {
                                ResultObj.FailCount = ResultObj.FailCount + 1;
                            }
                        }
                        ResultObj.TestCaseCount = ResultObj.TestCaseCount + 1;
                    }
                    double passPercentageBasedExec;
                    double passPercentageBasedPlanned;
                    double failPercentage;
                    double executedPercentage;
                    double executed = ResultObj.PassCount + ResultObj.FailCount;
                    if (executed != 0)
                    {
                        passPercentageBasedExec = ((ResultObj.PassCount / executed) * 100);
                    }
                    else
                    {
                        passPercentageBasedExec = 0;
                    }
                    if (ResultObj.PassCount != 0 && ResultObj.TestCaseCount != 0)
                    {
                        passPercentageBasedPlanned = ((ResultObj.PassCount / ResultObj.TestCaseCount) * 100);
                    }
                    else
                    {
                        passPercentageBasedPlanned = 0;
                    }
                    if (executed != 0)
                    {
                        failPercentage = ((float)(ResultObj.FailCount / executed) * 100);
                    }
                    else
                    {
                        failPercentage = 0;
                    }
                    if (ResultObj.TestCaseCount != 0)
                    {
                        executedPercentage = ((executed / ResultObj.TestCaseCount) * 100);
                    }
                    else
                    {
                        executedPercentage = 0;
                    }
                    double notExecuted = ResultObj.TestCaseCount - executed;

                    int passPercentageBasedExecConvert = Convert.ToInt32(passPercentageBasedExec);
                    int passPercentageBasedPlannedConvert = Convert.ToInt32(passPercentageBasedPlanned);
                    int failPercentageConvert = Convert.ToInt32(failPercentage);
                    int executedPercentageConvert = Convert.ToInt32(executedPercentage);

                    TestResult DataResult = ResultStore(inf.name, ResultObj.TestCaseCount.ToString(), ResultObj.PassCount,
                                                      ResultObj.FailCount, executed.ToString(), passPercentageBasedExec,
                                                      passPercentageBasedPlanned, notExecuted.ToString(),
                                                      executedPercentage.ToString());

                    TestList.Add(DataResult);
                    ResultObj.PassCount = 0;
                    ResultObj.FailCount = 0;
                    ResultObj.TestCaseCount = 0;
                }

                //  ViewBag.datastore = dataretrive;
                ViewBag.data1234 = dataretrive;
                Session["responsedata"] = dataretrive;
                //     return View("TestDisplay");
            }
            catch
            {

            }
            
            return Json(TestList, JsonRequestBehavior.AllowGet);
        }

        private TestResult ResultStore(string suitename, string planned, double passCount, double failCount, string executed, double passPercentageBasedExec, double passPercentageBasedPlanned, string notExecuted, string executedPercentage)
        {
            TestResult ResultObjListStore = new TestResult();
            ResultObjListStore.Suite = suitename;
            ResultObjListStore.Planned = planned;
            ResultObjListStore.Pass = passCount.ToString();
            ResultObjListStore.Fail = failCount.ToString();
            ResultObjListStore.Executed = executed;
            ResultObjListStore.PassBasedOnExecution = passPercentageBasedExec.ToString();
            ResultObjListStore.PassPercentage = passPercentageBasedPlanned.ToString();
            ResultObjListStore.NotYetExecution = notExecuted.ToString();
            ResultObjListStore.ExecutedPercentage = executedPercentage.ToString();

            return ResultObjListStore;
        }

    }
    public class ListClass
    {
        public static List<TestResult> TestList = new List<TestResult>();
        public static List<TestRunById> RunList = new List<TestRunById>();
    }
    public class TestResult
    {
        public string Suite { get; set; }
        public string Planned { get; set; }
        public string Executed { get; set; }
        public string ExecutedPercentage { get; set; }
        public string Pass { get; set; }
        public string Fail { get; set; }
        public string PassPercentage { get; set; }
        public string PassBasedOnExecution { get; set; }
        public string NotYetExecution { get; set; }


        public double PassCount { get; set; }
        public double FailCount { get; set; }
        public double TestCaseCount { get; set; }


    }
}