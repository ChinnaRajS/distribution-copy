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
  public class CLAccount : AzureDevOpsOAuthConfigs
    {
        //private string _baseUrl { get { return "https://app.vssps.visualstudio.com"; } }
        private string _token { get; set; }
        private string _profileId { get; set; }
        public CLAccount(string accessToken,string profileId): base()
        {
            _token = accessToken;
            _profileId = profileId;
        }


        /// <summary>
        /// CreatedOn            :       12-12-2018
        /// Created By           :       Deviprasad
        /// Document Url         :       https://docs.microsoft.com/en-us/rest/api/azure/devops/account/accounts/list?view=azure-devops-rest-4.1
        /// Api Url              :       GET https://app.vssps.visualstudio.com/_apis/accounts?ownerId={ownerId}&memberId={memberId}&properties={properties}&api-version=4.1
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :       NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetListAccounts()
        {


            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = AuthConfig.AzureDevOpsBaseUrl;
            oAppConfigurations.UrlParams =string.Format("/_apis/accounts?ownerId={0}/api-version=4.1", _profileId);
            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }


    }
}
