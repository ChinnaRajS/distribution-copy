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
   public class CLMember : AzureDevOpsOAuthConfigs
    {

        private string _baseUrl { get { return "https://vsaex.dev.azure.com/"; } }
        private string _token { get; set; }
        public CLMember(string accessToken) : base()
        {
            _token = accessToken;
        }


        /// <summary>
        /// CreatedOn            :       
        /// Document Url         :      https://docs.microsoft.com/en-us/rest/api/azure/devops/memberentitlementmanagement/members/get?view=azure-devops-rest-5.0
        /// Api Url              :      GET https://vsaex.dev.azure.com/{organization}/_apis/GroupEntitlements/{groupId}/members?api-version=5.0-preview.1
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :      NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetMenbersForGroup(string organizationName, string groupId)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/_apis/GroupEntitlements/{1}/members?api-version=5.0-preview.1", organizationName, groupId);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }
    
    }
}
