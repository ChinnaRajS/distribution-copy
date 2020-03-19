using distribution_copy.Models.Model_AK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace distribution_copy.BL
{
  
    public class BussinessLogic
    {

        Org o1;
     //   Credentials accountInfo = new Credentials();

        public Profile profile()
        {

            string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Im9PdmN6NU1fN3AtSGpJS2xGWHo5M3VfVjBabyJ9.eyJuYW1laWQiOiIzZTQxNDRkMS1hMTFiLTYwMmQtOTc0NC0wNWRiNWE2OTBjN2YiLCJzY3AiOiJ2c28uYXVkaXRsb2cgdnNvLmJ1aWxkX2V4ZWN1dGUgdnNvLmNvZGVfZnVsbCB2c28uY29kZV9zdGF0dXMgdnNvLmNvbm5lY3RlZF9zZXJ2ZXIgdnNvLmRhc2hib2FyZHNfbWFuYWdlIHZzby5lbnRpdGxlbWVudHMgdnNvLmVudmlyb25tZW50X21hbmFnZSB2c28uZXh0ZW5zaW9uLmRhdGFfd3JpdGUgdnNvLmV4dGVuc2lvbl9tYW5hZ2UgdnNvLmdhbGxlcnlfYWNxdWlyZSB2c28uZ2FsbGVyeV9tYW5hZ2UgdnNvLmdyYXBoX21hbmFnZSB2c28uaWRlbnRpdHlfbWFuYWdlIHZzby5sb2FkdGVzdF93cml0ZSB2c28ubWFjaGluZWdyb3VwX21hbmFnZSB2c28ubWVtYmVyZW50aXRsZW1lbnRtYW5hZ2VtZW50IHZzby5ub3RpZmljYXRpb25fZGlhZ25vc3RpY3MgdnNvLm5vdGlmaWNhdGlvbl9tYW5hZ2UgdnNvLnBhY2thZ2luZ19tYW5hZ2UgdnNvLnByb2ZpbGVfd3JpdGUgdnNvLnByb2plY3RfbWFuYWdlIHZzby5yZWxlYXNlX21hbmFnZSB2c28uc2VjdXJlZmlsZXNfbWFuYWdlIHZzby5zZWN1cml0eV9tYW5hZ2UgdnNvLnNlcnZpY2VlbmRwb2ludF9tYW5hZ2UgdnNvLnN5bWJvbHNfbWFuYWdlIHZzby50YXNrZ3JvdXBzX21hbmFnZSB2c28udGVzdF93cml0ZSB2c28udG9rZW5hZG1pbmlzdHJhdGlvbiB2c28udG9rZW5zIHZzby52YXJpYWJsZWdyb3Vwc19tYW5hZ2UgdnNvLndpa2lfd3JpdGUgdnNvLndvcmtfZnVsbCIsImF1aSI6IjM2ZmNkMTkxLWE5ZTUtNGI0NS04M2FhLWU2YzE1MmU0NjcyNiIsImFwcGlkIjoiNDZhYTIxOWMtNmUwYy00ZDA1LWE0YzUtNDIyYjE2NDdiZjcwIiwiaXNzIjoiYXBwLnZzdG9rZW4udmlzdWFsc3R1ZGlvLmNvbSIsImF1ZCI6ImFwcC52c3Rva2VuLnZpc3VhbHN0dWRpby5jb20iLCJuYmYiOjE1ODIxOTc0NjEsImV4cCI6MTU4MjIwMTA2MX0.g712jYdztDcoGn70NgdMWjucAEYGWA_j5eNe8xXa34XKi81hSvFWhXeRQa8schmmo0UQEh-kXrhzLEmhd5LifL7KfDErFecfBgnxK_pB67fN8dh55tkodQ1M-48qVOpsJKGmZU6_0BYAc-cF_-RwAPds9tMlVGoN7iKyZHCklkvu79gLIXfBW07xBNHasTx4v9goXZqZieUuBEJFLEFU6C4oCxTypQ4tEf79TWOgMa2220n-NDhrG4-0oqWIk6FI-SeQNtyp83RRvB4roEVXXgIwOr2cQvVAQpcKW3Ju4f6FMGzR5s42HIpk19R8WEHY-Zu1NHEFKCcGYDujoJKDtg";
            // https://app.vssps.visualstudio.com/_apis/accounts?api-version=5.1
            string api = string.Format("https://app.vssps.visualstudio.com/_apis/accounts?memberId={0}?api-version=5.1", accessToken);

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
        public string MemberId;
        public string accessToken1 = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Im9PdmN6NU1fN3AtSGpJS2xGWHo5M3VfVjBabyJ9.eyJuYW1laWQiOiIzZTQxNDRkMS1hMTFiLTYwMmQtOTc0NC0wNWRiNWE2OTBjN2YiLCJzY3AiOiJ2c28uYXVkaXRsb2cgdnNvLmJ1aWxkX2V4ZWN1dGUgdnNvLmNvZGVfZnVsbCB2c28uY29kZV9zdGF0dXMgdnNvLmNvbm5lY3RlZF9zZXJ2ZXIgdnNvLmRhc2hib2FyZHNfbWFuYWdlIHZzby5lbnRpdGxlbWVudHMgdnNvLmVudmlyb25tZW50X21hbmFnZSB2c28uZXh0ZW5zaW9uLmRhdGFfd3JpdGUgdnNvLmV4dGVuc2lvbl9tYW5hZ2UgdnNvLmdhbGxlcnlfYWNxdWlyZSB2c28uZ2FsbGVyeV9tYW5hZ2UgdnNvLmdyYXBoX21hbmFnZSB2c28uaWRlbnRpdHlfbWFuYWdlIHZzby5sb2FkdGVzdF93cml0ZSB2c28ubWFjaGluZWdyb3VwX21hbmFnZSB2c28ubWVtYmVyZW50aXRsZW1lbnRtYW5hZ2VtZW50IHZzby5ub3RpZmljYXRpb25fZGlhZ25vc3RpY3MgdnNvLm5vdGlmaWNhdGlvbl9tYW5hZ2UgdnNvLnBhY2thZ2luZ19tYW5hZ2UgdnNvLnByb2ZpbGVfd3JpdGUgdnNvLnByb2plY3RfbWFuYWdlIHZzby5yZWxlYXNlX21hbmFnZSB2c28uc2VjdXJlZmlsZXNfbWFuYWdlIHZzby5zZWN1cml0eV9tYW5hZ2UgdnNvLnNlcnZpY2VlbmRwb2ludF9tYW5hZ2UgdnNvLnN5bWJvbHNfbWFuYWdlIHZzby50YXNrZ3JvdXBzX21hbmFnZSB2c28udGVzdF93cml0ZSB2c28udG9rZW5hZG1pbmlzdHJhdGlvbiB2c28udG9rZW5zIHZzby52YXJpYWJsZWdyb3Vwc19tYW5hZ2UgdnNvLndpa2lfd3JpdGUgdnNvLndvcmtfZnVsbCIsImF1aSI6ImU1Yjc3NzJmLWU0ZWUtNDBhNC04ZmY2LTgyYWQxMDM1M2I1NSIsImFwcGlkIjoiNDZhYTIxOWMtNmUwYy00ZDA1LWE0YzUtNDIyYjE2NDdiZjcwIiwiaXNzIjoiYXBwLnZzdG9rZW4udmlzdWFsc3R1ZGlvLmNvbSIsImF1ZCI6ImFwcC52c3Rva2VuLnZpc3VhbHN0dWRpby5jb20iLCJuYmYiOjE1ODIxOTU0NTMsImV4cCI6MTU4MjE5OTA1M30.MP0KdhieQJACD95gynaTFMY7JHKlTz8GllbZVmOg16Q-o4lJbrjwO0TRHc0wlJYoKS_k2o1BoyEO1l5YvzSNVOw7T7tUtVhK1XLGXe8O4FIPmgOQNZ3yALW6L1DWGj3qfvVoYKxm0RJAVbSOjsN_FoXGWl696RFYD9J2oRBf4iWIBvThAVdz6MlUyAOgeTfgGKLUcIqi-vD-WF0SgJepTsyHkhlFcnWG4aR0JD4cs1vLyDiMfWPI17ZsyM8uAEecgkcoeCe0GAhczdgzfCfDxcNuQTmKLaL92b-FKQEHmoMPHfAAGQuAHP-tlljXUXVLDbjUuCuQsHnBxrKpmv4Yjg";
        public Organization Organization(string memberId)
        {
            // string memberId = "3e4144d1-a11b-602d-9744-05db5a690c7f";
            Organization testData = new Organization();
            try
            {
                string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Im9PdmN6NU1fN3AtSGpJS2xGWHo5M3VfVjBabyJ9.eyJuYW1laWQiOiIzZTQxNDRkMS1hMTFiLTYwMmQtOTc0NC0wNWRiNWE2OTBjN2YiLCJzY3AiOiJ2c28uYXVkaXRsb2cgdnNvLmJ1aWxkX2V4ZWN1dGUgdnNvLmNvZGVfZnVsbCB2c28uY29kZV9zdGF0dXMgdnNvLmNvbm5lY3RlZF9zZXJ2ZXIgdnNvLmRhc2hib2FyZHNfbWFuYWdlIHZzby5lbnRpdGxlbWVudHMgdnNvLmVudmlyb25tZW50X21hbmFnZSB2c28uZXh0ZW5zaW9uLmRhdGFfd3JpdGUgdnNvLmV4dGVuc2lvbl9tYW5hZ2UgdnNvLmdhbGxlcnlfYWNxdWlyZSB2c28uZ2FsbGVyeV9tYW5hZ2UgdnNvLmdyYXBoX21hbmFnZSB2c28uaWRlbnRpdHlfbWFuYWdlIHZzby5sb2FkdGVzdF93cml0ZSB2c28ubWFjaGluZWdyb3VwX21hbmFnZSB2c28ubWVtYmVyZW50aXRsZW1lbnRtYW5hZ2VtZW50IHZzby5ub3RpZmljYXRpb25fZGlhZ25vc3RpY3MgdnNvLm5vdGlmaWNhdGlvbl9tYW5hZ2UgdnNvLnBhY2thZ2luZ19tYW5hZ2UgdnNvLnByb2ZpbGVfd3JpdGUgdnNvLnByb2plY3RfbWFuYWdlIHZzby5yZWxlYXNlX21hbmFnZSB2c28uc2VjdXJlZmlsZXNfbWFuYWdlIHZzby5zZWN1cml0eV9tYW5hZ2UgdnNvLnNlcnZpY2VlbmRwb2ludF9tYW5hZ2UgdnNvLnN5bWJvbHNfbWFuYWdlIHZzby50YXNrZ3JvdXBzX21hbmFnZSB2c28udGVzdF93cml0ZSB2c28udG9rZW5hZG1pbmlzdHJhdGlvbiB2c28udG9rZW5zIHZzby52YXJpYWJsZWdyb3Vwc19tYW5hZ2UgdnNvLndpa2lfd3JpdGUgdnNvLndvcmtfZnVsbCIsImF1aSI6IjM2ZmNkMTkxLWE5ZTUtNGI0NS04M2FhLWU2YzE1MmU0NjcyNiIsImFwcGlkIjoiNDZhYTIxOWMtNmUwYy00ZDA1LWE0YzUtNDIyYjE2NDdiZjcwIiwiaXNzIjoiYXBwLnZzdG9rZW4udmlzdWFsc3R1ZGlvLmNvbSIsImF1ZCI6ImFwcC52c3Rva2VuLnZpc3VhbHN0dWRpby5jb20iLCJuYmYiOjE1ODIxOTc0NjEsImV4cCI6MTU4MjIwMTA2MX0.g712jYdztDcoGn70NgdMWjucAEYGWA_j5eNe8xXa34XKi81hSvFWhXeRQa8schmmo0UQEh-kXrhzLEmhd5LifL7KfDErFecfBgnxK_pB67fN8dh55tkodQ1M-48qVOpsJKGmZU6_0BYAc-cF_-RwAPds9tMlVGoN7iKyZHCklkvu79gLIXfBW07xBNHasTx4v9goXZqZieUuBEJFLEFU6C4oCxTypQ4tEf79TWOgMa2220n-NDhrG4-0oqWIk6FI-SeQNtyp83RRvB4roEVXXgIwOr2cQvVAQpcKW3Ju4f6FMGzR5s42HIpk19R8WEHY-Zu1NHEFKCcGYDujoJKDtg";
                // https://app.vssps.visualstudio.com/_apis/accounts?api-version=5.1
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
                string Pat = Org.pat;
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

                        //List<SelectListItem> proList = new List<SelectListItem>();
                        //foreach (var data in Org1.ProjectNameList.value)
                        //{
                        //    proList.Add(new SelectListItem { Text = data.name, Value = data.name });
                        //}
                        //Org1.DropdownList = proList;
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
                string OrganizationName = "aniruddhajere"; //Org.OrganizationName;
                string Pat = "gqkh3ielshhknu5j64v6jjcylsczx6yt7g2qfks6sb6spgyc23ra"; //Org.pat;
                string BaseAddress = "https://dev.azure.com/";
                string projectName = "Mindtree_testcase_Copy"; //Org.ProjectName;

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
                string OrganizationName = Org.OrganizationName;
                string Pat = Org.pat;
                string BaseAddress = "https://dev.azure.com/";
                string projectName = Org.ProjectName;

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

                        // return obj;
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
                string OrganizationName = Org.OrganizationName;
                string Pat = Org.pat;
                string BaseAddress = "https://dev.azure.com/";
                string projectName = Org.ProjectName;

                string api = string.Format("https://dev.azure.com/{0}/{1}/_apis/testplan/Plans/{2}/Suites/{3}/TestCase?api-version=5.1-preview.2", Org.OrganizationName, Org.ProjectName, testPlanId, suitId);//TempCredintials.OrganizationName,TempCredintials.ProjectName,testPlanId, suitId);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//TempCredintials.pat ); //Org.pat

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


        //Get Test Run By Project

        public TestRunProject GetRunByProject()
        {
            TestRunProject testData = new TestRunProject();
            string api = string.Format("https://dev.azure.com/{0}/{1}//_apis/test/runs?api-version=5.0", Org.OrganizationName, Org.ProjectName);//TempCredintials.OrganizationName,TempCredintials.ProjectName,testPlanId, suitId);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//TempCredintials.pat ); //Org.pat

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;
                    testData = JsonConvert.DeserializeObject<TestRunProject>(obj);
                }
            }
            return testData;
        }

        public TestRunById GetRunByRunId(string runId)
        {

            TestRunById testData = new TestRunById();
            string api = string.Format("https://dev.azure.com/{0}/{1}//_apis/test/Runs/{2}/results?api-version=5.0", Org.OrganizationName, Org.ProjectName, runId);//TempCredintials.OrganizationName,TempCredintials.ProjectName,testPlanId, suitId);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//TempCredintials.pat ); //Org.pat

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    var obj = response.Content.ReadAsStringAsync().Result;
                    testData = JsonConvert.DeserializeObject<TestRunById>(obj);
                }
            }
            return testData;
        }

    }
}