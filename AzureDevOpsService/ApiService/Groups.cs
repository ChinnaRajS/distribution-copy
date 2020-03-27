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
    class CLGroup : AzureDevOpsOAuthConfigs
    {

        private string _baseUrl { get { return "https://vssps.dev.azure.com"; } }
        private string _token { get; set; }
        public CLGroup(string accessToken) : base()
        {
            _token = accessToken;
        }


        /// <summary>
        /// CreatedOn            :       
        /// Document Url         :      https://docs.microsoft.com/en-us/rest/api/azure/devops/graph/groups/list?view=azure-devops-rest-5.0
        /// Api Url              :      GET https://vssps.dev.azure.com/{organization}/_apis/graph/groups?api-version=5.0-preview.1
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :      NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetAllGroups(string organizationName)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/_apis/graph/groups?api-version=5.0-preview.1", organizationName);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }


        /// <summary>
        /// CreatedOn            :       
        /// Document Url         :     https://docs.microsoft.com/en-us/rest/api/azure/devops/graph/groups/list?view=azure-devops-rest-5.0
        /// Api Url              :      GET https://vssps.dev.azure.com/{organization}/_apis/graph/groups/{groupDescriptor}?api-version=5.0-preview.1
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :      NO
        /// </summary>
        /// <returns></returns>

        public ApiResponseMsg GetGroups(string organizationName,string scopeDescriptor="")
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            if (string.IsNullOrEmpty(scopeDescriptor))
            {
                oAppConfigurations.UrlParams = string.Format("{0}/_apis/graph/groups?api-version=5.0-preview.1", organizationName);
            }
            else
            {
                oAppConfigurations.UrlParams = string.Format("{0}/_apis/graph/groups?scopeDescriptor={1}&api-version=5.0-preview.1", organizationName,scopeDescriptor);
            }
            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = Constants.SchemeOAuth;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }

    }
}
