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
    public class CLProfile : AzureDevOpsOAuthConfigs
    {
        //private string _baseUrl { get { return "https://app.vssps.visualstudio.com"; } }
        private string _token { get; set; }
        public CLProfile(string accessToken) : base()
        {
            _token = accessToken;
        }


        /// <summary>
        /// CreatedOn            :       
        /// Document Url         :      https://docs.microsoft.com/en-us/rest/api/azure/devops/profile/profiles/get?view=azure-devops-rest-4.1
        /// Api Url              :      GET https://app.vssps.visualstudio.com/_apis/profile/profiles/{id}?details={details}&withAttributes={withAttributes}&partition={partition}&coreAttributes={coreAttributes}&forceRefresh={forceRefresh}&api-version=4.1
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :      NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetCurrentProfile()
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = AuthConfig.AzureDevOpsBaseUrl;
            oAppConfigurations.UrlParams = "_apis/profile/profiles/me?api-version=4.1";
            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }
    }
}
