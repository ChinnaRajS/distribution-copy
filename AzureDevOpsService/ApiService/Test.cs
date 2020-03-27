using AzureDevOpsService.Helpers;
using AzureDevOpsService.HttpService;
using AzureDevOpsService.Models;

namespace AzureDevOpsService.ApiService
{
    public class ADOCLTest : AzureDevOpsOAuthConfigs
    {
        private string _baseUrl { get { return "https://dev.azure.com"; } }
        private string _token { get; set; }
        private string _profileId { get; set; }
        public ADOCLTest(string accessToken) : base()
        {
            _token = accessToken;
            //  _profileId = profileId;
        }


        /// <summary>
        /// CreatedOn            :       
        /// Created By           :       
        /// Document Url         :      
        /// Api Url              :      
        ///                             
        /// Discription          :      
        /// Current Version      :      
        /// BasicTest Executed   :       NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetTestPlans(string organizationName, string projectName, bool includePlanDetails = true)
        {


            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            if (includePlanDetails)
            {
                oAppConfigurations.UrlParams = string.Format("/{0}/{1}/_apis/test/plans?includePlanDetails={2}&api-version=5.0", organizationName, projectName, includePlanDetails);
            }
            else
            {
                oAppConfigurations.UrlParams = string.Format("/{0}/{1}/_apis/test/plans?api-version=5.0", organizationName, projectName);
            }

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }

        /// <summary>
        /// CreatedOn            :       
        /// Created By           :       
        /// Document Url         :    GET https://dev.azure.com/{organization}/{project}/_apis/testplan/Plans/{planId}/suites?api-version=5.0-preview.1  
        /// Api Url              :      
        ///                             
        /// Discription          :      
        /// Current Version      :      
        /// BasicTest Executed   :       NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetTestSuitesByTestPlanId(string organizationName, string projectName, string testPlanId)
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;
            oAppConfigurations.UrlParams = string.Format("/{0}/{1}/_apis/testplan/Plans/{2}/suites?api-version=5.0-preview.1  ", organizationName, projectName, testPlanId);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }


        /// <summary>
        /// CreatedOn            :       
        /// Created By           :       
        /// Document Url         :      
        /// Api Url              :      GET https://dev.azure.com/{organization}/{project}/_apis/test/Plans/{planId}/suites/{suiteId}?api-version=5.0
        ///                             
        /// Discription          :      
        /// Current Version      :      
        /// BasicTest Executed   :       NO
        /// </summary>
        /// <returns></returns>
        public ApiResponseMsg GetTestCasesByPlanIdAndSuiteId(string organizationName, string projectName, string planId, string suiteId)
        {


            HttpConfigurations oAppConfigurations = new HttpConfigurations();

            oAppConfigurations.BaseUrl = _baseUrl;

            oAppConfigurations.UrlParams = string.Format("/{0}/{1}/_apis/test/plans/{2}/suites/{3}/testcases?api-version=5.0", organizationName, projectName, planId, suiteId);

            oAppConfigurations.SecurityKey = _token;
            oAppConfigurations.ContentType = Constants.ContentTypeJson;
            oAppConfigurations.HttpMethod = Constants.Get;
            oAppConfigurations.AuthenticationScheme = AuthConfig.AuthType;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Get();

        }



    }
}
