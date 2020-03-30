using AzureDevOpsService.Helpers;
using AzureDevOpsService.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AzureDevOpsService.ApiService
{
    public class CLTestCaseReport
    {
        public ExcelPackage ExportTestCases(string token, string accountName = "", string projectName = "")
        {
            try
            {
                accountName = "tooldemo";
                projectName = "tooldemoproject";

                VMRootTestCase vmRootTestCase = new VMRootTestCase();
                VMProject vmProject = new VMProject();
                vmProject.Plans = new List<VMPlan>();
                List<string> wiIdList = new List<string>();
                string wiIds = string.Empty;
                int wiIDCount = 0;
                CLProfile profile = new CLProfile(token);
                CLWorkItem workItem = new CLWorkItem(token);

                var profiles = profile.GetCurrentProfile();
                CLAccount account = new CLAccount(token, Convert.ToString(profiles.ResponseAsDynamicObj.id));
                var accounts = account.GetListAccounts();

                List<CLMAccount> acc = new List<CLMAccount>();
                acc = JsonConvert.DeserializeObject<List<CLMAccount>>(accounts.ResponseAsString);
                var accountDetails = acc.FirstOrDefault(x => x.AccountName == accountName);



                ADOCLProjects projects = new ADOCLProjects(token);
                var getProjectResponse = projects.GetProjects(accountDetails.AccountName);
                List<Value> projectDetails = new List<Value>();
                projectDetails = JsonConvert.DeserializeObject<List<Value>>(JsonConvert.SerializeObject(getProjectResponse.ResponseAsDynamicObj.value));

                var projectDetail = projectDetails.FirstOrDefault(x => x.name == projectName);

                vmProject.Id = projectDetail.id;
                vmProject.Name = projectDetail.name;

                ADOCLTest testPlan = new ADOCLTest(token);

                var testPlanResponse = testPlan.GetTestPlans(accountDetails.AccountName, projectDetail.name);
                //deCounter = testPlanResponse.ResponseAsDynamicObj.value.Count;
                foreach (var testPan in testPlanResponse.ResponseAsDynamicObj.value)
                {
                    var vmTestPlan = new VMPlan();
                    vmTestPlan.Id = Convert.ToString(testPan.id);
                    vmTestPlan.Name = Convert.ToString(testPan.name);
                    vmTestPlan.Suites = new List<VMSuite>();
                    var testSuites = testPlan.GetTestSuitesByTestPlanId(accountDetails.AccountName, projectDetail.name, vmTestPlan.Id);
                    foreach (var suite in testSuites.ResponseAsDynamicObj.value)
                    {
                        var vmTestSuites = new VMSuite();
                        vmTestSuites.Id = Convert.ToString(suite.id);
                        vmTestSuites.Name = Convert.ToString(suite.name);
                        vmTestSuites.TestCases = new List<VMTestcase>();

                        var testCases = testPlan.GetTestCasesByPlanIdAndSuiteId(accountDetails.AccountName, projectDetail.name, vmTestPlan.Id, vmTestSuites.Id);

                        foreach (var testCase in testCases.ResponseAsDynamicObj.value)
                        {
                            wiIDCount++;
                            var vmTestCase = new VMTestcase();
                            vmTestCase.Id = Convert.ToInt32(testCase.testCase.id);
                            wiIds = wiIds + "," + vmTestCase.Id;
                            vmTestSuites.TestCases.Add(vmTestCase);
                        }
                        vmTestPlan.Suites.Add(vmTestSuites);
                        //break;
                    }

                    vmProject.Plans.Add(vmTestPlan);
                    //break;
                }

                wiIds = wiIds.Trim(',');
                var commanSeparatedWIs = wiIds.WIAsList();

                foreach (var wItemIds in commanSeparatedWIs)
                {
                    var getWIResponse = workItem.GetWIByIds(accountDetails.AccountName, projectDetail.name, wItemIds);
                    var steps = JsonConvert.DeserializeObject<CLTestCase.CLTestCaseDetail>(getWIResponse.ResponseAsString);

                    foreach (var wi in steps.value)
                    {
                        foreach (var plans in vmProject.Plans)
                        {
                            foreach (var suites in plans.Suites)
                            {
                                foreach (var testcases in suites.TestCases)
                                {

                                    var testCases = suites.TestCases.FirstOrDefault(x => x.Id == wi.id);
                                    if (testCases != null)
                                    {

                                        suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Name = wi.fields.Title;
                                        suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Steps = new List<Step>();
                                        if (wi.fields.Steps != null)
                                        {
                                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                            doc.LoadXml(wi.fields.Steps);
                                            string jsonText = JsonConvert.SerializeXmlNode(doc).Replace(@"&nbsp;", "").Replace(@"@", "").Replace(@"#", "").Replace(@"<DIV>", "").Replace(@"<\DIV>", "");

                                            var t = Regex.Replace(jsonText, "<.*?>", String.Empty);

                                            var steps1 = JsonConvert.DeserializeObject<CLSteps.CLStepsTemplate>(t);
                                            foreach (var stepParams in steps1.Steps.step)
                                            {
                                                var newSteps = new Step();
                                                newSteps.Id = stepParams.id;
                                                newSteps.StepNames = new List<string>();
                                                foreach (var stepItem in stepParams.parameterizedString)
                                                {
                                                    newSteps.StepNames.Add(stepItem.text);
                                                }
                                                suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Steps.Add(newSteps);
                                            }

                                        }

                                    }

                                }
                            }
                        }

                    }
                }

                using (ExcelPackage xp = new ExcelPackage())
                {
                    string jsom = JsonConvert.SerializeObject(vmProject);
                    int startRow = 2;

                    var workSheet = xp.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Cells[1, 1, 1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1].Value = "PlanName";
                    workSheet.Cells[1, 2].Value = "Suite";
                    workSheet.Cells[1, 3].Value = "TestCase";
                    workSheet.Cells[1, 4].Value = "Steps";
                    workSheet.Cells[1, 5].Value = "Expected Result";
                    int startStyle = 2;
                    //bool toggleColor = true;
                    foreach (var plans in vmProject.Plans)
                    {
                        startStyle = startRow;
                        workSheet.Cells[startRow, 1].Value = plans.Name;
                        workSheet.Cells[startRow, 1].AutoFitColumns();// = true;
                        //workSheet.Cells[startRow, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Justify;
                        //workSheet.Cells[startRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                        foreach (var suites in plans.Suites)
                        {
                            bool stepsExists = false;
                            workSheet.Cells[startRow, 2].Value = suites.Name;

                            workSheet.Cells[startRow, 2].AutoFitColumns();// = true;
                            //workSheet.Cells[startRow, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Justify;
                            //workSheet.Cells[startRow, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                            foreach (var testcases in suites.TestCases)
                            {

                                workSheet.Cells[startRow, 3].Value = testcases.Name;

                                workSheet.Cells[startRow, 3].AutoFitColumns();// = true;
                                //workSheet.Cells[startRow, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Justify;
                                //workSheet.Cells[startRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify; ;
                                int i = 0;
                                foreach (var stepParams in testcases.Steps)
                                {
                                    stepsExists = true;
                                    bool isExpectedResult = false;
                                    foreach (var stepItem in stepParams.StepNames)
                                    {
                                        if (!isExpectedResult)
                                        {
                                            workSheet.Cells[startRow, 4].Value = "Step " + i + " : " + stepItem;
                                            workSheet.Cells[startRow, 4].AutoFitColumns();// = true;
                                            //workSheet.Cells[startRow, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Justify;
                                            //workSheet.Cells[startRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                                            isExpectedResult = true;
                                            i++;
                                        }
                                        else
                                        {
                                            workSheet.Cells[startRow, 5].Value = stepItem;
                                            workSheet.Cells[startRow, 5].AutoFitColumns();// = true;
                                            //workSheet.Cells[startRow, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Justify;
                                            //workSheet.Cells[startRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                                            isExpectedResult = false;
                                        }

                                    }

                                    if (stepParams.StepNames.Count > 0)
                                    {
                                        startRow++;
                                    }

                                }

                                startRow++;
                            }
                            if (!stepsExists)
                            {
                                startRow++;
                            }


                        }


                        //workSheet.Cells[startStyle, 1, startRow, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //if (toggleColor)
                        //{
                        //    toggleColor = false;

                        //    workSheet.Cells[startStyle, 1, startRow, 5].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                        //    workSheet.Cells[startStyle, 1, startRow, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                        //}
                        //else
                        //{
                        //    toggleColor = true; ;
                        //    workSheet.Cells[startStyle, 1, startRow, 5].Style.Fill.BackgroundColor.SetColor(Color.Lavender);
                        //    workSheet.Cells[startStyle, 1, startRow, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                        //}
                        startRow++;
                    }
                    return xp;
                    // xp.SaveAs(new System.IO.FileInfo(string.Format(@"D:\excel\{0}.xlsx", Guid.NewGuid().ToString())));
                }


                //return null;
            }
            catch (Exception)
            {

                return null;
            }



        }

        public string ExportTestSteps(string token, string accountName = "", string projectName = "")
        {
            try
            {
                string exportPath = string.Empty;
                string environment = ConfigurationManager.AppSettings["environment"];
                if (environment == Constants.DevEnvironment)
                {
                    accountName = "tooldemo";
                    projectName = "tooldemoproject";
                    token = ConfigurationManager.AppSettings["PAT"];
                }
                VMRootTestCase vmRootTestCase = new VMRootTestCase();
                VMProject vmProject = new VMProject();
                vmProject.Plans = new List<VMPlan>();
                List<string> wiIdList = new List<string>();
                string wiIds = string.Empty;
                int wiIDCount = 0;
                CLProfile profile = new CLProfile(token);
                CLWorkItem workItem = new CLWorkItem(token);


                ADOCLProjects projects = new ADOCLProjects(token);
                var getProjectResponse = projects.GetProjects(accountName);
                List<Value> projectDetails = new List<Value>();
                projectDetails = JsonConvert.DeserializeObject<List<Value>>(JsonConvert.SerializeObject(getProjectResponse.ResponseAsDynamicObj.value));

                var projectDetail = projectDetails.FirstOrDefault(x => x.name.ToLower() == projectName.ToLower());

                vmProject.Id = projectDetail.id;
                vmProject.Name = projectDetail.name;

                ADOCLTest testPlan = new ADOCLTest(token);

                var testPlanResponse = testPlan.GetTestPlans(accountName, projectDetail.name);
                foreach (var testPan in testPlanResponse.ResponseAsDynamicObj.value)
                {
                    var vmTestPlan = new VMPlan();
                    vmTestPlan.Id = Convert.ToString(testPan.id);
                    vmTestPlan.Name = Convert.ToString(testPan.name);
                    vmTestPlan.Suites = new List<VMSuite>();
                    var testSuites = testPlan.GetTestSuitesByTestPlanId(accountName, projectDetail.name, vmTestPlan.Id);
                    foreach (var suite in testSuites.ResponseAsDynamicObj.value)
                    {
                        var vmTestSuites = new VMSuite();
                        vmTestSuites.Id = Convert.ToString(suite.id);
                        vmTestSuites.Name = Convert.ToString(suite.name);
                        vmTestSuites.TestCases = new List<VMTestcase>();

                        var testCases = testPlan.GetTestCasesByPlanIdAndSuiteId(accountName, projectDetail.name, vmTestPlan.Id, vmTestSuites.Id);

                        foreach (var testCase in testCases.ResponseAsDynamicObj.value)
                        {
                            wiIDCount++;
                            var vmTestCase = new VMTestcase();
                            vmTestCase.Id = Convert.ToInt32(testCase.testCase.id);
                            wiIds = wiIds + "," + vmTestCase.Id;
                            vmTestSuites.TestCases.Add(vmTestCase);
                        }
                        vmTestPlan.Suites.Add(vmTestSuites);
                        //break;
                    }

                    vmProject.Plans.Add(vmTestPlan);
                    //break;
                }

                wiIds = wiIds.Trim(',');
                var commanSeparatedWIs = wiIds.WIAsList();

                foreach (var wItemIds in commanSeparatedWIs)
                {
                    var getWIResponse = workItem.GetWIByIds(accountName, projectDetail.name, wItemIds);
                    var steps = JsonConvert.DeserializeObject<CLTestCase.CLTestCaseDetail>(getWIResponse.ResponseAsString);

                    foreach (var wi in steps.value)
                    {
                        foreach (var plans in vmProject.Plans)
                        {
                            foreach (var suites in plans.Suites)
                            {
                                foreach (var testcases in suites.TestCases)
                                {

                                    var testCases = suites.TestCases.FirstOrDefault(x => x.Id == wi.id);
                                    if (testCases != null)
                                    {

                                        suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Name = wi.fields.Title;
                                        suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Steps = new List<Step>();
                                        if (wi.fields.Steps != null)
                                        {
                                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                            doc.LoadXml(wi.fields.Steps);
                                            string jsonText = JsonConvert.SerializeXmlNode(doc).Replace(@"&nbsp;", "").Replace(@"@", "").Replace(@"#", "").Replace(@"<DIV>", "").Replace(@"<\DIV>", "");

                                            var t = Regex.Replace(jsonText, "<.*?>", String.Empty);

                                            var steps1 = JsonConvert.DeserializeObject<CLSteps.CLStepsTemplate>(t);
                                            foreach (var stepParams in steps1.Steps.step)
                                            {
                                                var newSteps = new Step();
                                                newSteps.Id = stepParams.id;
                                                newSteps.StepNames = new List<string>();
                                                foreach (var stepItem in stepParams.parameterizedString)
                                                {
                                                    newSteps.StepNames.Add(stepItem.text);
                                                }
                                                suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Steps.Add(newSteps);
                                            }

                                        }

                                    }

                                }
                            }
                        }

                    }
                }

                using (ExcelPackage xp = new ExcelPackage())
                {
                    string jsom = JsonConvert.SerializeObject(vmProject);
                    int startRow = 2;
                    int totalColCount = 5;
                    var workSheet = xp.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Cells[1, 1, 1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1].Value = "PlanName";
                    workSheet.Cells[1, 2].Value = "Suite";
                    workSheet.Cells[1, 3].Value = "TestCase";
                    workSheet.Cells[1, 4].Value = "Steps";
                    workSheet.Cells[1, 5].Value = "Expected Result";
                    int startStyle = 2;
                    foreach (var plans in vmProject.Plans)
                    {
                        startStyle = startRow;
                        workSheet.Cells[startRow, 1].Value = plans.Name;
                        foreach (var suites in plans.Suites)
                        {
                            bool stepsExists = false;
                            workSheet.Cells[startRow, 2].Value = suites.Name;
                            foreach (var testcases in suites.TestCases)
                            {

                                workSheet.Cells[startRow, 3].Value = testcases.Name;
                                int i = 1;
                                foreach (var stepParams in testcases.Steps)
                                {
                                    stepsExists = true;
                                    bool isExpectedResult = false;
                                    foreach (var stepItem in stepParams.StepNames)
                                    {
                                        if (!isExpectedResult)
                                        {
                                            workSheet.Cells[startRow, 4].Value = "Step " + i + " : " + stepItem;
                                            isExpectedResult = true;
                                            i++;
                                        }
                                        else
                                        {
                                            workSheet.Cells[startRow, 5].Value = stepItem;
                                            isExpectedResult = false;
                                        }
                                    }

                                    if (stepParams.StepNames.Count > 0)
                                    {
                                        startRow++;
                                    }
                                }
                                startRow++;
                            }
                            if (!stepsExists)
                            {
                                startRow++;
                            }


                        }
                        startRow++;
                    }

                    for (int i = 1; i < totalColCount + 1; i++)
                    {
                        workSheet.Column(i).AutoFit();
                    }
                    exportPath = DriveInfo.GetDrives()[0]+ @"ExportExcel\";
                    if (!Directory.Exists(exportPath))
                    {
                        Directory.CreateDirectory(exportPath);
                    }
                    exportPath = string.Format(@"{0}{1}_{2}.xlsx", exportPath,projectName, DateTime.Now.ToString("yyyyMMddHHssss"));
                    xp.SaveAs(new System.IO.FileInfo(exportPath));
                }


                return exportPath;
            }
            catch (Exception)
            {
                return Constants.Failed;

            }



        }

        public VMProject TestCaseDetails(string token, string accountName = "", string projectName = "")
        {
            try
            {
                accountName = "tooldemo";
                projectName = "tooldemoproject";

                VMRootTestCase vmRootTestCase = new VMRootTestCase();
                VMProject vmProject = new VMProject();
                vmProject.Plans = new List<VMPlan>();
                List<string> wiIdList = new List<string>();
                string wiIds = string.Empty;
                int wiIDCount = 0;
                CLProfile profile = new CLProfile(token);
                CLWorkItem workItem = new CLWorkItem(token);

                var profiles = profile.GetCurrentProfile();
                CLAccount account = new CLAccount(token, Convert.ToString(profiles.ResponseAsDynamicObj.id));
                var accounts = account.GetListAccounts();

                List<CLMAccount> acc = new List<CLMAccount>();
                acc = JsonConvert.DeserializeObject<List<CLMAccount>>(accounts.ResponseAsString);
                var accountDetails = acc.FirstOrDefault(x => x.AccountName == accountName);



                ADOCLProjects projects = new ADOCLProjects(token);
                var getProjectResponse = projects.GetProjects(accountDetails.AccountName);
                List<Value> projectDetails = new List<Value>();
                projectDetails = JsonConvert.DeserializeObject<List<Value>>(JsonConvert.SerializeObject(getProjectResponse.ResponseAsDynamicObj.value));

                var projectDetail = projectDetails.FirstOrDefault(x => x.name == projectName);

                vmProject.Id = projectDetail.id;
                vmProject.Name = projectDetail.name;

                ADOCLTest testPlan = new ADOCLTest(token);

                var testPlanResponse = testPlan.GetTestPlans(accountDetails.AccountName, projectDetail.name);
                //deCounter = testPlanResponse.ResponseAsDynamicObj.value.Count;
                foreach (var testPan in testPlanResponse.ResponseAsDynamicObj.value)
                {
                    var vmTestPlan = new VMPlan();
                    vmTestPlan.Id = Convert.ToString(testPan.id);
                    vmTestPlan.Name = Convert.ToString(testPan.name);
                    vmTestPlan.Suites = new List<VMSuite>();
                    var testSuites = testPlan.GetTestSuitesByTestPlanId(accountDetails.AccountName, projectDetail.name, vmTestPlan.Id);
                    foreach (var suite in testSuites.ResponseAsDynamicObj.value)
                    {
                        var vmTestSuites = new VMSuite();
                        vmTestSuites.Id = Convert.ToString(suite.id);
                        vmTestSuites.Name = Convert.ToString(suite.name);
                        vmTestSuites.TestCases = new List<VMTestcase>();

                        var testCases = testPlan.GetTestCasesByPlanIdAndSuiteId(accountDetails.AccountName, projectDetail.name, vmTestPlan.Id, vmTestSuites.Id);

                        foreach (var testCase in testCases.ResponseAsDynamicObj.value)
                        {
                            wiIDCount++;
                            var vmTestCase = new VMTestcase();
                            vmTestCase.Id = Convert.ToInt32(testCase.testCase.id);
                            wiIds = wiIds + "," + vmTestCase.Id;
                            vmTestSuites.TestCases.Add(vmTestCase);
                        }
                        vmTestPlan.Suites.Add(vmTestSuites);
                        //break;
                    }

                    vmProject.Plans.Add(vmTestPlan);
                    //break;
                }

                wiIds = wiIds.Trim(',');
                var commanSeparatedWIs = wiIds.WIAsList();

                foreach (var wItemIds in commanSeparatedWIs)
                {
                    var getWIResponse = workItem.GetWIByIds(accountDetails.AccountName, projectDetail.name, wItemIds);
                    var steps = JsonConvert.DeserializeObject<CLTestCase.CLTestCaseDetail>(getWIResponse.ResponseAsString);

                    foreach (var wi in steps.value)
                    {
                        foreach (var plans in vmProject.Plans)
                        {
                            foreach (var suites in plans.Suites)
                            {
                                foreach (var testcases in suites.TestCases)
                                {

                                    var testCases = suites.TestCases.FirstOrDefault(x => x.Id == wi.id);
                                    if (testCases != null)
                                    {

                                        suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Name = wi.fields.Title;
                                        suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Steps = new List<Step>();
                                        if (wi.fields.Steps != null)
                                        {
                                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                            doc.LoadXml(wi.fields.Steps);
                                            string jsonText = JsonConvert.SerializeXmlNode(doc).Replace(@"&nbsp;", "").Replace(@"@", "").Replace(@"#", "").Replace(@"<DIV>", "").Replace(@"<\DIV>", "");

                                            var t = Regex.Replace(jsonText, "<.*?>", String.Empty);

                                            var steps1 = JsonConvert.DeserializeObject<CLSteps.CLStepsTemplate>(t);
                                            foreach (var stepParams in steps1.Steps.step)
                                            {
                                                var newSteps = new Step();
                                                newSteps.Id = stepParams.id;
                                                newSteps.StepNames = new List<string>();
                                                foreach (var stepItem in stepParams.parameterizedString)
                                                {
                                                    newSteps.StepNames.Add(stepItem.text);
                                                }
                                                suites.TestCases.FirstOrDefault(x => x.Id == wi.id).Steps.Add(newSteps);
                                            }

                                        }

                                    }

                                }
                            }
                        }

                    }
                }



                return vmProject;
            }
            catch (Exception)
            {

                return null;
            }



        }

        private static void removeAllAttributes(XDocument doc)
        {
            foreach (var des in doc.Descendants())
            {
                des.RemoveAttributes();
            }
        }

        public string GetGroups(string token, string accountName = "", string projectName = "")
        {
            try
            {
                String headersConfig = ConfigurationManager.AppSettings["AreaHeaders"];
                string roleConfig = ConfigurationManager.AppSettings["UserRoles"];

                var roles = roleConfig.Split(',');
                var headers = headersConfig.Split(',');
                int totalColomnsCount = roles.Count() + headers.Count();
                accountName = "naveenkunder";
                projectName = "smarthotel";
                CLWorkItemTracking clWorkItemTracking = new CLWorkItemTracking(token);
                CLMember clMembers = new CLMember(token);
                CLGroup clGroups = new CLGroup(token);
                CLProfile profile = new CLProfile(token);
                CLDescriptor clDescriptor = new CLDescriptor(token);
                List<Area> areas = new List<Area>();

                var profiles = profile.GetCurrentProfile();
                CLAccount account = new CLAccount(token, Convert.ToString(profiles.ResponseAsDynamicObj.id));
                var accounts = account.GetListAccounts();

                List<CLMAccount> acc = new List<CLMAccount>();
                acc = JsonConvert.DeserializeObject<List<CLMAccount>>(accounts.ResponseAsString);
                var accountDetails = acc.FirstOrDefault(x => x.AccountName.ToLower() == accountName.ToLower());

                ADOCLProjects projects = new ADOCLProjects(token);
                var getProjectResponse = projects.GetProjects(accountDetails.AccountName);
                List<Value> projectDetails = new List<Value>();
                projectDetails = JsonConvert.DeserializeObject<List<Value>>(JsonConvert.SerializeObject(getProjectResponse.ResponseAsDynamicObj.value));

                var projectDetail = projectDetails.FirstOrDefault(x => x.name.ToLower() == projectName.ToLower());

                var descriptors = clDescriptor.GetDescriptors(accountDetails.AccountName, projectDetail.id);

                List<MemberGroup> memberGroups = new List<MemberGroup>();
                var groupsResult = clGroups.GetGroups(accountName, Convert.ToString(descriptors.ResponseAsDynamicObj.value));
                foreach (var group in groupsResult.ResponseAsDynamicObj.value)
                {
                    MemberGroup memberGroup = new MemberGroup();
                    memberGroup.GroupPrincipalName = Convert.ToString(group.principalName);
                    memberGroup.GroupDisplayName = Convert.ToString(group.displayName);
                    var d = memberGroup.GroupDisplayName.Split('_');
                    if (d.Count() > 0)
                    {
                        memberGroup.TempAreaName = d[0];
                    }

                    memberGroup.GroupMembers = new List<GroupMember>();
                    var members = clMembers.GetMenbersForGroup(accountDetails.AccountName, Convert.ToString(group.originId));
                    foreach (var member in members.ResponseAsDynamicObj.members)
                    {
                        GroupMember groupMember = new GroupMember();
                        groupMember.MemberUserId = Convert.ToString(member.user.mailAddress);
                        groupMember.MemberName = Convert.ToString(member.user.displayName);
                        memberGroup.GroupMembers.Add(groupMember);
                    }
                    memberGroups.Add(memberGroup);

                }

                var rootArea = new Area.RootArea();
                rootArea.Areas = new List<Area.AreaDetail>();

                var areaPath = clWorkItemTracking.GetClassificationNodes(accountName, projectName);
                var classficationModel = new AreaClassification();
                classficationModel = JsonConvert.DeserializeObject<AreaClassification>(areaPath.ResponseAsString);
                using (ExcelPackage xp = new ExcelPackage())
                {
                    int startStyle = 2;
                    int startRow = 2;
                    var workSheet = xp.Workbook.Worksheets.Add("Sheet1");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Cells[1, 1, 1, totalColomnsCount].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                    foreach (var header in headers)
                    {
                        var splitHeader = header.Split(':');
                        workSheet.Cells[1, Convert.ToInt32(splitHeader[1])].Value = splitHeader[0];
                    }

                    foreach (var role in roles)
                    {
                        var roleCol = role.Split(':');
                        workSheet.Cells[1, Convert.ToInt32(roleCol[1])].Value = roleCol[0];
                        workSheet.Column(Convert.ToInt32(roleCol[1])).AutoFit();
                    }

                    foreach (var area in classficationModel.value)
                    {
                        if (area.structureType == "area")
                        {
                            startStyle = startRow;
                            Area.AreaDetail areaDetail = new Area.AreaDetail();
                            if (area.hasChildren)
                            {
                                foreach (var projectArea in area.children)
                                {
                                    workSheet.Cells[startRow, 1].Value = projectArea.name;
                                    if (projectArea.hasChildren)
                                    {
                                        //SubArea Code
                                        foreach (var subArea in projectArea.children)
                                        {
                                            workSheet.Cells[startRow, 2].Value = subArea.name;
                                            bool isMemberExists = false;
                                            var usersD = memberGroups.Where(x => x.TempAreaName == subArea.name).ToList();
                                            Dictionary<string, int> dict = new Dictionary<string, int>();

                                            foreach (var group in usersD)
                                            {
                                                string actulalRole = group.GroupDisplayName.Split('_').Count() > 0 ? group.GroupDisplayName.Split('_')[1] : group.GroupDisplayName; ;
                                                foreach (var member in group.GroupMembers)
                                                {
                                                    isMemberExists = true;
                                                    if (dict.ContainsKey(member.MemberUserId))
                                                    {

                                                        var val = dict[member.MemberUserId];
                                                        foreach (var role in roles)
                                                        {
                                                            var roleCol = role.Split(':');
                                                            if (roleCol[2] == actulalRole)
                                                            {
                                                                workSheet.Cells[val, Convert.ToInt32(roleCol[1])].Value = actulalRole;
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        workSheet.Cells[startRow, 3].Value = member.MemberName;
                                                        workSheet.Cells[startRow, 4].Value = member.MemberUserId;
                                                        workSheet.Cells[startRow, 1].Value = projectArea.name;
                                                        workSheet.Cells[startRow, 2].Value = subArea.name;

                                                        foreach (var role in roles)
                                                        {
                                                            var roleCol = role.Split(':');
                                                            if (roleCol[2] == actulalRole)
                                                            {
                                                                workSheet.Cells[startRow, Convert.ToInt32(roleCol[1])].Value = actulalRole;
                                                            }
                                                        }
                                                        dict.Add(member.MemberUserId, startRow);
                                                        startRow++;
                                                    }
                                                }
                                            }
                                            if (!isMemberExists)
                                            {
                                                workSheet.Cells[startRow, 1].Value = projectArea.name;
                                                workSheet.Cells[startRow, 2].Value = subArea.name;

                                                startRow++;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Area code
                                        var areaUsers = memberGroups.Where(x => x.TempAreaName == projectArea.name).ToList();
                                        Dictionary<string, int> dict = new Dictionary<string, int>();
                                        foreach (var group in areaUsers)
                                        {
                                            bool isMemberExists = false;
                                            string actulalRole = group.GroupDisplayName.Split('_').Count() > 0 ? group.GroupDisplayName.Split('_')[1] : group.GroupDisplayName; ;
                                            foreach (var member in group.GroupMembers)
                                            {
                                                isMemberExists = true;
                                                if (dict.ContainsKey(member.MemberUserId))
                                                {

                                                    var val = dict[member.MemberUserId];
                                                    foreach (var role in roles)
                                                    {
                                                        var roleCol = role.Split(':');
                                                        if (roleCol[2] == actulalRole)
                                                        {
                                                            workSheet.Cells[val, Convert.ToInt32(roleCol[1])].Value = actulalRole;
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    workSheet.Cells[startRow, 3].Value = member.MemberName;
                                                    workSheet.Cells[startRow, 4].Value = member.MemberUserId;
                                                    workSheet.Cells[startRow, 1].Value = projectArea.name;
                                                    foreach (var role in roles)
                                                    {
                                                        var roleCol = role.Split(':');
                                                        if (roleCol[2] == actulalRole)
                                                        {
                                                            workSheet.Cells[startRow, Convert.ToInt32(roleCol[1])].Value = actulalRole;
                                                        }

                                                    }
                                                    dict.Add(member.MemberUserId, startRow);
                                                    startRow++;
                                                }
                                            }

                                            if (!isMemberExists)
                                            {
                                                startRow++;
                                                //workSheet.Cells[startRow, 1].Value = projectArea.name;
                                            }

                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    for (int i = 1; i <= totalColomnsCount; i++)
                    {
                        workSheet.Column(i).AutoFit();
                    }

                    xp.SaveAs(new System.IO.FileInfo(string.Format(@"D:\excel\{0}.xlsx", Guid.NewGuid().ToString())));
                }


                return "";
            }
            catch (Exception)
            {

                return null;
            }



        }


    }
}
