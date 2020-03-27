using distribution_copy.Models.Model_AK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace distribution_copy.BL
{
    public class ClientRequest
    {

        public System.Net.Http.HttpResponseMessage ClientRequestMethod(string jsonStringData, string accessToken, string api)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonContent = new StringContent(jsonStringData, Encoding.UTF8, "application/json");//"application/json-patch+json");
                    var method = new HttpMethod("POST");
                    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TempCredintials.pat);//accessToken);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);
                    var request = new HttpRequestMessage(method, api) { Content = jsonContent };
                    var response = client.SendAsync(request).Result;
                    var Message = response.Content.ReadAsStringAsync();

                    return response;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public System.Net.Http.HttpResponseMessage GetResponseMethod(string api)
        {
            TestPlan testData = new TestPlan();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Org.pat);//Org.pat);
                    HttpResponseMessage response = client.GetAsync(api).Result;
                    return response;
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

    }
}