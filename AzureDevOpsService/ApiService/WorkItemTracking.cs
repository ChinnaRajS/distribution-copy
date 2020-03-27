using AzureDevOpsService.Helpers;
using AzureDevOpsService.HttpService;
using AzureDevOpsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsService.ApiService
{
    public class CLWorkItemTracking : AzureDevOpsOAuthConfigs
    {

        private string _baseUrl { get { return "https://dev.azure.com/"; } }
        private string _token { get; set; }
        public CLWorkItemTracking(string accessToken) : base()
        {
            _token = accessToken;
        }


        /// <summary>
        /// CreatedOn            :       
        /// Document Url         :      https://docs.microsoft.com/en-us/rest/api/azure/devops/wit/classification%20nodes/get?view=azure-devops-rest-5.0
        /// Api Url              :      GET https://dev.azure.com/{organization}/SMH360/_apis/wit/classificationnodes?$depth=3&api-version=5.0 
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :      NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetClassificationNodes(string organizationName, string projectName)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/{1}/_apis/wit/classificationnodes?$depth=3&api-version=5.0", organizationName, projectName);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }
    }
}
