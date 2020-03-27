using AzureDevOpsService.Helpers;
using AzureDevOpsService.HttpService;
using AzureDevOpsService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace AzureDevOpsService.ApiService
{
    public class CLWorkItem : AzureDevOpsOAuthConfigs
    {
        private string _baseUrl { get { return "https://dev.azure.com"; } }
        private string _token { get; set; }
        public CLWorkItem(string accessToken) : base()
        {
            _token = accessToken;
        }

        public ApiResponseMsg CreateWorkItem(string organization, string workItemReqBody)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = AuthConfig.AzureDevOpsBaseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/_apis/work/processes?api-version=4.1-preview.1", organization);
            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Post;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            oAppConfigurations.RequestBody = workItemReqBody;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }
        public ApiResponseMsg GetWIByIds(string organizationName, string projectName, string ids)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/{1}/_apis/wit/workitems?ids={2}&$expand=relations&api-version=5.0", organizationName, projectName, ids);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }

        public ApiResponseMsg GetWorkItemTypes(string organizationName, string projectName)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/{1}//_apis/wit/workitemtypes?api-version=5.0", organizationName, projectName);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();
        }

        public ApiResponseMsg GetWorkItemsByQuery(string organizationName, string projectName)
        {
            Object wiql = new
            {
                query = "Select [Id],[State], [Title]" +
                        "From WorkItems " +
                        //"Where [Work Item Type] = '" + workItemType + "'" +
                        "Where [System.TeamProject] = '" + projectName + "' " +
                        "Order By [Stack Rank] Desc, [Backlog Priority] Desc"
            };

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("/{0}/_apis/wit/wiql?api-version=5.0", organizationName);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.RequestBody = JsonConvert.SerializeObject(wiql);
            oAppConfigurations.HttpMethod = Constants.Post;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Post();
        }

        public List<string> WIAsList(string value)
        {
            List<string> values = new List<string>();
            if (!string.IsNullOrEmpty(value))
            {
                string tempVal = string.Empty;
                int count = 0;
                bool isAdded = false;
                var arryList = value.Split(',');
                foreach (var id in arryList)
                {
                    count++;
                    tempVal = tempVal + "," + id;
                    if (count > 0)
                    {
                        isAdded = false;
                    }

                    if (count >= 200)
                    {
                        tempVal = tempVal.Trim(',');
                        values.Add(tempVal);
                        count = 0;
                        isAdded = true;
                        tempVal = string.Empty;
                    }
                }
                if (!isAdded)
                {
                    tempVal = tempVal.Trim(',');
                    values.Add(tempVal);
                }
            }
            return values;
        }

        public List<string> WIAsList(dynamic value)
        {
            int toalWI = 0;
            List<string> values = new List<string>();
            if (value != null)
            {
                string tempVal = string.Empty;
                int count = 0;
                bool isAdded = false;

                foreach (var id in value)
                {
                    toalWI++;
                    count++;
                    tempVal = tempVal + "," + Convert.ToString(id.id);
                    if (count > 0)
                    {
                        isAdded = false;
                    }

                    if (count >= 200)
                    {
                        tempVal = tempVal.Trim(',');
                        values.Add(tempVal);
                        count = 0;
                        isAdded = true;
                        tempVal = string.Empty;
                    }
                }
                if (!isAdded)
                {
                    tempVal = tempVal.Trim(',');
                    values.Add(tempVal);
                }
            }
            return values;
        }

        public System.Drawing.Image ExportAttachments(string accountName, string projectName, string attachmentId, string attachmentName)
        {

            System.Drawing.Image image = null;

            try
            {
                HttpConfigurations oAppConfigurations = new HttpConfigurations();
                oAppConfigurations.BaseUrl = _baseUrl;
                oAppConfigurations.UrlParams = string.Format("{0}/{1}/_apis/wit/attachments/{2}?download=true&api-version=5.0", accountName, projectName, attachmentId);

                oAppConfigurations.SecurityKey = _token;
                oAppConfigurations.ContentType = Constants.ContentTypeJson;
                oAppConfigurations.HttpMethod = Constants.Get;
                oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
                HttpServices oHttpService = new HttpServices(oAppConfigurations);

                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(oAppConfigurations.BaseUrl +"/"+ oAppConfigurations.UrlParams);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Authorization", oAppConfigurations.AuthenticationScheme + " " + oAppConfigurations.SecurityKey);
                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            return image;
        }
        public Byte[] DownloadAttachment(string accountName, string projectName, string attachmentId, string attachmentName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/octet-stream"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                    return client.GetByteArrayAsync(string.Format("{0}/{1}/_apis/wit/attachments/{2}?api-version=5.0", accountName, projectName, attachmentId)).Result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
