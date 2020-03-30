using AzureDevOpsService.Helpers;
using AzureDevOpsService.HttpService;
using AzureDevOpsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace AzureDevOpsService.ApiService
{
    public class ADOCLProjects : AzureDevOpsOAuthConfigs
    {
        private string _baseUrl { get { return "https://dev.azure.com"; } }
        private string _token { get; set; }
        private string _profileId { get; set; }
        public ADOCLProjects(string accessToken) : base()
        {
            _token = accessToken;
          //  _profileId = profileId;
        }


        /// <summary>
        /// CreatedOn            :       12-12-2018
        /// Created By           :       Deviprasad
        /// Document Url         :       https://docs.microsoft.com/en-us/rest/api/azure/devops/account/accounts/list?view=azure-devops-rest-4.1
        /// Api Url              :       GET https://dev.azure.com/{organization}/_apis/projects?api-version=5.0
        ///                              GET https://dev.azure.com/{organization}/_apis/projects?stateFilter={stateFilter}&$top={$top}&$skip={$skip}&continuationToken={continuationToken}&getDefaultTeamImageUrl={getDefaultTeamImageUrl}&api-version=5.0
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :       NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetProjects(string organizationName,string stateFilter="")
        {


            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("/{0}/_apis/projects?api-version=4.1", organizationName);
            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }
    }
}
