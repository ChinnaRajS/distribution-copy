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
    public class CLDescriptor : AzureDevOpsOAuthConfigs
    {

        private string _baseUrl { get { return "https://vssps.dev.azure.com"; } }
        private string _token { get; set; }
        public CLDescriptor(string accessToken) : base()
        {
            _token = accessToken;
        }


        /// <summary>
        /// CreatedOn            :       
        /// Document Url         :      
        /// Api Url              :      GET https://vssps.dev.azure.com/{organization}/_apis/graph/descriptors/{storageKey}?api-version=5.0-preview.1
        /// Discription          :       
        /// Current Version      :       
        /// BasicTest Executed   :      NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetDescriptors(string organizationName,string projectId)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("{0}/_apis/graph/descriptors/{1}?api-version=5.0-preview.1", organizationName, projectId);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Helpers.Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }

    }
}
