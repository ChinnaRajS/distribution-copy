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
        public ActionResult TestRun(string testCaseId)
        {
            List<TestRunPartial> testrunStoreList = new List<TestRunPartial>();
            List<TestRunById> testrunList = new List<TestRunById>();
            TestRunById testrun = new TestRunById();
            List<TestRunById> runStore;
            List<TestRunById> RunList = new List<TestRunById>();
            TestRunProject testProData = logic.GetRunByProject();
            foreach (var test in testProData.value)
            {
                TestRunById runData = logic.GetRunByRunId(test.id.ToString());
                RunList.Add(runData);
                ListClass.RunList.Add(runData);
            }
            //project level run
            TestSuit dataretrive = new TestSuit();

            runStore = new List<TestRunById>();
            //TestCasesFromSuits testresult = logic.TestCaseFromSuit(plan.id.ToString(), inf.id.ToString);
            WorkItemsClass testresult = logic.testCaseMethod(testCaseId);

            //foreach (var test in testresult.fields)
            //{
            foreach (var run in RunList)
            {
                foreach (var run1 in run.value)
                {
                    if (run1.testCase.id.ToString() == testCaseId)//testresult.id.ToString())
                    {
                        testrun = new TestRunById();
                        testrun = run;
                        testrunList.Add(testrun);
                        //int runid = Convert.ToInt32(run1.testRun.id);
                    }
                }
                // }
            }
            foreach (var test in testrunList)
            {
                foreach (var test1 in test.value)
                {
                    TestRunPartial store = testStoreMethod(Convert.ToInt32(test1.testRun.id), test1.testRun.name, test1.outcome);
                    testrunStoreList.Add(store);
                }
            }
            return PartialView("_TestRun",testrunStoreList);
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



        //public JsonResult TestCasesFromTestSuit(string testPlanId, string testSuitId)
        //{
        //    TestCasesFromSuits testresult = logic.TestCaseFromSuit(testPlanId, testSuitId);
        //    return Json(testresult, JsonRequestBehavior.AllowGet);
        //}
        
        List<TestCaseStoreSub> dataStoreList = new List<TestCaseStoreSub>();
        public JsonResult TestCasesFromTestSuit(string testPlanId, string testSuitId)
        {
            TestCasesFromSuits testresult = logic.TestCaseFromSuit(testPlanId, testSuitId);
            //
            TestRunById testrun = new TestRunById();
            List<TestRunById> RunList = new List<TestRunById>();
            TestRunProject testProData = logic.GetRunByProject();
            foreach (var test in testProData.value)
            {
                TestRunById runData = logic.GetRunByRunId(test.id.ToString());
                RunList.Add(runData);
                ListClass.RunList.Add(runData);
            }

            string outcome1 = string.Empty;
            foreach (var test in testresult.value)
            {
                TestCaseStoreSub dataStore1 = new TestCaseStoreSub();
                List<int> maxStore = new List<int>();
                // List<string> runStore = new List<string>();

                foreach (var run in RunList)
                {
                    foreach (var run1 in run.value)
                    {
                        if (run1.testCase.id == null || run1.testCase.id == "")
                        {
                        }
                        else
                        {
                            if (run1.testCase.id.ToString() == test.workItem.id.ToString())
                            {
                                //TestResult.TestCaseList.Add(testresult);
                                //TestResult.TestRun.Add(run);
                                int runid = Convert.ToInt32(run1.testRun.id);
                                maxStore.Add(runid);
                                // runStore.Add(run);
                            }
                        }
                    }
                }
                if (maxStore == null || maxStore.Count <= 0)
                {
                    outcome1 = "Not Yet Runned";
                }
                    if (maxStore != null && maxStore.Count > 0)
                {
                    var maxId = maxStore.Max();
                    //RunList.Find();
                    // TestRunById dataQuery =new TestRunById();

                    foreach (var i in RunList)
                    {
                        //  var dataQuery = i.value.Find(x => x.testRun.id == maxId.ToString());
                        foreach (var k in i.value)
                        {
                            if (k.testRun.id == maxId.ToString())
                            {
                                if (k.outcome != null)
                                {
                                    outcome1 = k.outcome.ToString();
                                    //dataStore1.ID = test.workItem.id.ToString();
                                    //dataStore1.Name = test.workItem.name.ToString();
                                    //dataStore1.Outcome = outcome1;
                                }
                                else
                                {
                                    outcome1 = "Not Yet runned";
                                }
                            }

                        }
                    }
                }
                dataStore1= caseResultStore(test.workItem.id.ToString(), test.workItem.name.ToString(),outcome1);
                
                dataStoreList.Add(dataStore1);
            }

            //
            return Json(dataStoreList, JsonRequestBehavior.AllowGet);
        }
        public TestCaseStoreSub caseResultStore(string id, string name, string outcome)
        {
            TestCaseStoreSub dataStore12 = new TestCaseStoreSub();
            dataStore12.ID = id;
            dataStore12.Name = name;
            dataStore12.Outcome = outcome;
            return dataStore12;
        }
        public JsonResult TestRunFromTestCase(string testCaseId)
        {
            List<TestRunPartial> testrunStoreList = new List<TestRunPartial>();
            TestRunById testrun=new TestRunById();
            List<TestRunById> testrunList = new List<TestRunById>();
            List<TestRunById> runStore;
            List<TestRunById> RunList = new List<TestRunById>();
            //List<TestRunById> RundetailsList = new List<TestRunById>();
            TestRunProject testProData = logic.GetRunByProject();
            foreach (var test in testProData.value)
            {
                TestRunById runData = new TestRunById();
                runData = logic.GetRunByRunId(test.id.ToString());
                RunList.Add(runData);
                ListClass.RunList.Add(runData);
            }
            //project level run
            TestSuit dataretrive = new TestSuit();

            runStore = new List<TestRunById>();
            //TestCasesFromSuits testresult = logic.TestCaseFromSuit(plan.id.ToString(), inf.id.ToString);
            WorkItemsClass testresult = logic.testCaseMethod(testCaseId);

                //foreach (var test in testresult.fields)
                //{
                    foreach (var run in RunList)
                    {
                        foreach (var run1 in run.value)
                        {
                            if (run1.testCase.id.ToString() == testCaseId)//testresult.id.ToString())
                            {
                            testrun = new TestRunById();
                            testrun = run;
                            testrunList.Add(testrun);
                            //int runid = Convert.ToInt32(run1.testRun.id);
                            }
                        }
                // }
                }
            foreach (var test in testrunList)
            {
                foreach (var test1 in test.value)
                {
                    TestRunPartial store = testStoreMethod(test1.id, test1.testCaseTitle, test1.outcome);
                    testrunStoreList.Add(store);
                }
            }
            return Json(testrunStoreList, JsonRequestBehavior.AllowGet);
        }
        
        public TestRunPartial testStoreMethod(int id, string testCaseTitle, string outcome)
        {
            TestRunPartial TestClass = new TestRunPartial();
            TestClass.id = id.ToString();
            TestClass.name = testCaseTitle;
            TestClass.outcome = outcome;
            return TestClass;
        }

      
        public JsonResult GetTestSuits(string planid)
        {
            List<TestRunById> runStore;
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
                    runStore = new List<TestRunById>();
                    TestCasesFromSuits testresult = logic.TestCaseFromSuit(inf.plan.id.ToString(), inf.id.ToString());
                    foreach (var test in testresult.value)
                    {
                        List<int> maxStore = new List<int>();
                        // List<string> runStore = new List<string>();
                        
                        foreach (var run in RunList)
                        {
                            foreach (var run1 in run.value)
                            {                                
                                    if (run1.testCase.id.ToString() == test.workItem.id.ToString())
                                    {
                                   // TestResult.TestCaseList.Add(testresult);
                                    //TestResult.TestRun.Add(run);
                                    int runid = Convert.ToInt32(run1.testRun.id);
                                        maxStore.Add(runid);
                                    runStore.Add(run);
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
                        failPercentage = ((ResultObj.FailCount / executed) * 100);
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

                    int passPercentageBasedExecConvert = (int)Math.Round(passPercentageBasedExec);//Convert.ToInt32(passPercentageBasedExec);
                    int passPercentageBasedPlannedConvert = (int)Math.Round(passPercentageBasedPlanned);//Convert.ToInt32(passPercentageBasedPlanned);
                    int failPercentageConvert = Convert.ToInt32(failPercentage);
                    int executedPercentageConvert = (int)Math.Round(executedPercentage);//Convert.ToInt32(executedPercentage);

                    //  i = (int)Math.Round(x / y);

                    TestResult DataResult = ResultStore(inf.id.ToString(),inf.name, ResultObj.TestCaseCount.ToString(), ResultObj.PassCount,
                                                      ResultObj.FailCount, executed.ToString(), passPercentageBasedExecConvert,
                                                      passPercentageBasedPlannedConvert, notExecuted.ToString(),
                                                      executedPercentageConvert);
                    DataResult.TestRun=runStore;
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
            catch(Exception ex)
            {
            }            
            return Json(TestList, JsonRequestBehavior.AllowGet);
        }

        private TestResult ResultStore(string suitId,string suitename, string planned, double passCount, double failCount, string executed, int passPercentageBasedExec, int passPercentageBasedPlanned, string notExecuted, int executedPercentage)
        {
            TestResult ResultObjListStore = new TestResult();
            ResultObjListStore.SuiteId = suitId;
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
        public string SuiteId { get; set; }
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

        public List<TestCasesFromSuits> TestCaseList = new List<TestCasesFromSuits>();
        public List<TestRunById> TestRun = new List<TestRunById>();
    }

    public class TestCaseStoreSub
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Outcome { get; set; }

    }

    public class TestRunPartial
    {
        public string id { get; set; }
        public string name { get; set; }
        public string outcome { get; set; }
    }
}