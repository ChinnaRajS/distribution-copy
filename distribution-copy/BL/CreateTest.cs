using distribution_copy.Models.Model_AK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace distribution_copy.BL
{
    public class CreateTest
    { 
  public TestCaseResponseModel TestCaseCreate(WorkItemsClass testcases)
    {
        TestCaseResponseModel Result = new TestCaseResponseModel();
        try
        {

            string testName = testcases.fields.Title;
            string jsonString = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\TestCaseJson.json");
            jsonString = jsonString.Replace("$name$", testName);

            string api = string.Format("https://dev.azure.com/{0}/{1}/_apis/wit/workitems/${2}?api-version=5.1", Org.OrganizationName, Org.ProjectName, "Test Case");
            using (var client = new HttpClient())
            {
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json-patch+json");//"application/json");
                var method = new HttpMethod("POST");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);
                var request = new HttpRequestMessage(method, api) { Content = jsonContent };
                var response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;

                    var Message = response.Content.ReadAsStringAsync();
                    Result = JsonConvert.DeserializeObject<TestCaseResponseModel>(obj);
                    return Result;
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync();

                    return Result;
                }

            }
            return Result;
        }
        catch
        {
            return Result;
        }
    }



    //Adding TestCase in the TestSuit

    public string AddtoTestCaseToTestSuit(string planId, string suitId, string testcaseId)
    {
        try
        {
            string jsonString = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~") + @"\Jsons\TestCaseJson.json");

            //https://dev.azure.com/aniruddhajere/Mindtree_testcase_Copy/_apis/test/Plans/3157/suites/3160/testcases/3379?api-version=5.1
            string api = string.Format("https://dev.azure.com/{0}/{1}/_apis/test/Plans/{2}/suites/{3}/testcases/{4}?api-version=5.1", Org.OrganizationName, Org.ProjectName, planId, suitId, testcaseId);
            using (var client = new HttpClient())
            {
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json-patch+json");//"application/json");
                var method = new HttpMethod("POST");
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);

                var request = new HttpRequestMessage(method, api);//"https://dev.azure.com/aniruddhajere/Mindtree_testcase_Copy/_apis/testplan/Plans/3157/Suites/3190/TestCases/304?api-version=5.1-preview.2");// { Content = jsonContent };   //3190  //3157
                var response = client.SendAsync(request).Result;
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    CheckValid.AddTestcase = true;
                }
                else
                {
                    CheckValid.AddTestcase = false;
                }
                if (response.IsSuccessStatusCode)
                {
                    var Message = response.Content.ReadAsStringAsync();
                    return null;
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync();

                    return null;
                }
            }
        }
        catch
        {
            return null;
        }
    }
}
}