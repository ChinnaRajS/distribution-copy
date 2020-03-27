using distribution_copy.Models.Model_AK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace distribution_copy.BL
{
    public class BussinessLogicCopy
    {
        public string MemberId;
        public Profile profile()
    {

        

        string api = string.Format("https://app.vssps.visualstudio.com/_apis/accounts?memberId={0}?api-version=5.1", Org.pat);

        Profile testData = new Profile();
        using (var client = new HttpClient())
        {
            try
            {
                string baseAddress = "https://app.vssps.visualstudio.com/";

                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//Org.pat);
                HttpResponseMessage response = client.GetAsync("_apis/profile/profiles/me?api-version=4.1").Result;
                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;
                    testData = JsonConvert.DeserializeObject<Profile>(obj);
                    MemberId = testData.id;



                }
            }
            catch (Exception ex)
            {

            }
        }
        return testData;
    }
    
    
    public Organization Organization(string memberId)
    {

        Organization testData = new Organization();
        try
        {
            string api = string.Format("https://app.vssps.visualstudio.com/_apis/accounts?memberId={0}&api-version=5.1", memberId);


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//Org.pat);
                HttpResponseMessage response = client.GetAsync(api).Result;//("https://app.vssps.visualstudio.com/_apis/accounts?memberId=3e4144d1-a11b-602d-9744-05db5a690c7f&api-version=5.1").Result;

                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;


                    testData = JsonConvert.DeserializeObject<Organization>(obj);
                }
                else
                {
                    var Message = response.Content.ReadAsStringAsync();
                }
            }
        }
        catch (Exception ex)
        {
        }
        return testData;
    }

    public RespData ProjectNamesStore()
    {
        Org Org1 = new Org();
        try
        {
            string OrganizationName = Org.OrganizationName;
            string BaseAddress = "https://dev.azure.com/";
            string api = string.Format("{0}{1}/_apis/projects?api-version=5.0-preview.3", BaseAddress, OrganizationName);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//);accessToken1

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;



                    Org1.ProjectNameList = JsonConvert.DeserializeObject<RespData>(obj);

                    List<SelectListItem> proList = new List<SelectListItem>();
                    foreach (var data in Org1.ProjectNameList.value)
                    {
                        proList.Add(new SelectListItem { Text = data.name, Value = data.name });
                    }
                   // Org1.DropdownList = proList;
                }
            }
        }
        catch (Exception ex)
        {

        }
        return Org1.ProjectNameList;
    }

    public TestPlan JsonRetrive()
    {
        Org Org1 = new Org();
        TestPlan testData = new TestPlan();
        try
        {
            string BaseAddress = "https://dev.azure.com/";
            string api = string.Format("{0}{1}/{2}/_apis/test/plans?api-version=5.0", BaseAddress, Org.OrganizationName, Org.ProjectName);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//Org.pat);

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;

                    testData = JsonConvert.DeserializeObject<TestPlan>(obj);
                }
            }
        }
        catch (Exception ex)
        {

        }
        return testData;
    }

    public TestSuit TestSuit(string testPlanId)
    {
        Org Org1 = new Org();
        TestSuit testData = new TestSuit();
        try
        {
           
            string BaseAddress = "https://dev.azure.com/";
           
            string api = string.Format("{0}{1}/{2}/_apis/testplan/Plans/{3}/suites?api-version=5.0-preview.1", BaseAddress, Org.OrganizationName, Org.ProjectName, testPlanId);//TempCredintials.OrganizationName, TempCredintials.ProjectName, testPlanId);

            string pat = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Org.pat)));

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {

                    var obj = response.Content.ReadAsStringAsync().Result;
                    testData = JsonConvert.DeserializeObject<TestSuit>(obj);
                }
            }
        }
        catch (Exception ex)
        {

        }
        return testData;
    }

    public TestCasesFromSuits TestCaseFromSuit(string testPlanId, string suitId)
    {

        TestCasesFromSuits testData = new TestCasesFromSuits();
        try
        {
         
            string api = string.Format("https://dev.azure.com/{0}/{1}/_apis/testplan/Plans/{2}/Suites/{3}/TestCase?api-version=5.1-preview.2", Org.OrganizationName, Org.ProjectName, testPlanId, suitId);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {

                    var obj = response.Content.ReadAsStringAsync().Result;

                    testData = JsonConvert.DeserializeObject<TestCasesFromSuits>(obj);

                }
            }
        }
        catch (Exception ex)
        {
        }
        return testData;
    }



    public TestPlan TestPlanId(string testPlanId)
    {
        TestPlan Result = new TestPlan();
        try
        {

            string api = string.Format("https://dev.azure.com/{0}/{1}/_apis/testplan/plans/{2}?api-version=5.1-preview.1", Org.OrganizationName, Org.ProjectName, testPlanId);
            ClientRequest c1 = new ClientRequest();
            var response = c1.GetResponseMethod(api);
            if (response.IsSuccessStatusCode)
            {
                var obj = response.Content.ReadAsStringAsync().Result;

                var Message = response.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<TestPlan>(obj);
                return Result;
            }

            return Result;
        }
        catch
        {
            return Result;
        }
    }

    /// <summary>
    /// To Retrive TestCase with the testcase id
    /// </summary>
    /// <param name="testCaseId"></param>
    /// <returns></returns>
    public WorkItemsClass TestCaseRetrive(string testCaseId)
    {
        WorkItemsClass Result = new WorkItemsClass();
        try
        {
            string api = string.Format("https://dev.azure.com/{0}/{1}/_apis/wit/workitems/{2}?api-version=5.0", Org.OrganizationName, Org.ProjectName, testCaseId);
            ClientRequest c1 = new ClientRequest();
            var response = c1.GetResponseMethod(api);
            if (response.IsSuccessStatusCode)
            {
                var obj = response.Content.ReadAsStringAsync().Result;

                var Message = response.Content.ReadAsStringAsync();
                Result = JsonConvert.DeserializeObject<WorkItemsClass>(obj);
                return Result;
            }

            return Result;
        }
        catch
        {
            return Result;
        }
    }
}
}