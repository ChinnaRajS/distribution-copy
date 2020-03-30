
using AzureDevOpsService.Helpers;
using AzureDevOpsService.HttpService;
using AzureDevOpsService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzureDevOpsService.ApiService
{
    public class AzureDevOpsAuthorize : AzureDevOpsOAuthConfigs
    {
        public string _code { set; get; }
        private string _accessToken { get; set; }
        private string _refreshToken { get; set; }
        private string _response { get; set; }
        public AzureDevOpsAuthorize() : base()
        {

        }
        public AzureDevOpsAuthorize(string code) : base()
        {
            try
            {
                _code = code;
                var tokenDetails = GetAccessToken();
                _response = JsonConvert.SerializeObject(tokenDetails);
                _accessToken = Convert.ToString(tokenDetails.ResponseAsDynamicObj.access_token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string AccessToken()
        {
            return this._accessToken;
        }

        public string RespnseMessage()
        {
            return this._response;
        }

        private string _tokenUrl { get { return "/oauth2/token"; } }
        private string _authorizeUrl { get { return "/oauth2/authorize"; } }

        private ApiResponseMsg GetAccessToken()
        {

            HttpConfigurations oAppConfigurations = new HttpConfigurations();
            oAppConfigurations.BaseUrl = AuthConfig.AzureDevOpsBaseUrl;
            oAppConfigurations.UrlParams = _tokenUrl;
            oAppConfigurations.ContentType = Constants.ContentTypeUriEncode;
            oAppConfigurations.HttpMethod = Constants.Post;
            oAppConfigurations.RequestBody = GetTokenExchangeParam(_code);
            oAppConfigurations.AuthenticationScheme = Constants.ContentTypeUriEncode;
            HttpServices oHttpService = new HttpServices(oAppConfigurations);
            return oHttpService.Post(true);
        }

        
        public string Login()
        {
            
            return string.Format(AuthConfig.AzureDevOpsBaseUrl+ _authorizeUrl
                                         + AuthConfig.AzureDevOpsParams
                                         , AuthConfig.AzureDevOpsClientId
                                         , AuthConfig.AzureDevOpsReponseType
                                         , AuthConfig.AzureDevOpsReponseState
                                         , AuthConfig.TempScope
                                         , AuthConfig.AzureDevOpsRedirectionUri
                                         );

        }

        private string GetTokenExchangeParam(string authCode)
        {
            return string.Format(AuthConfig.AccessTokenParams, HttpContext.Current.Server.UrlEncode(AuthConfig.AzureDevOpsAppSecret), HttpContext.Current.Server.UrlEncode(authCode), AuthConfig.AzureDevOpsRedirectionUri);
        }

    }

    public abstract class AzureDevOpsOAuthConfigs
    {
        protected OAuthConfiguration AuthConfig { get; set; }
        protected AzureDevOpsOAuthConfigs()
        {
            AuthConfig = new OAuthConfiguration();
            AuthConfig.AuthType = ConfigurationManager.AppSettings["AuthType"];
            AuthConfig.Environment = ConfigurationManager.AppSettings["Environment"];
            AuthConfig.AzureDevOpsBaseUrl = ConfigurationManager.AppSettings["AzureDevOpsBaseUrl"];
            //if (AuthConfig.AuthType == Constants.BasicAuthentication)
            //{
                
                AuthConfig.AzureDevOpsParams = ConfigurationManager.AppSettings["AzureDevOpsParams"];
                AuthConfig.AzureDevOpsClientId = ConfigurationManager.AppSettings["AzureDevOpsClientId"];
                AuthConfig.AzureDevOpsScope = ConfigurationManager.AppSettings["AzureDevOpsScope"];
                AuthConfig.AzureDevOpsRedirectionUri = ConfigurationManager.AppSettings["AzureDevOpsRedirectionUri"];
                AuthConfig.AzureDevOpsReponseState = ConfigurationManager.AppSettings["AzureDevOpsReponseState"];
                AuthConfig.AzureDevOpsReponseType = ConfigurationManager.AppSettings["AzureDevOpsReponseType"];
                AuthConfig.AccessTokenParams = ConfigurationManager.AppSettings["AccessTokenParams"];
                AuthConfig.AzureDevOpsAppSecret = ConfigurationManager.AppSettings["AzureDevOpsAppSecret"];
                AuthConfig.TempScope = ConfigurationManager.AppSettings["TempScope"];
                //AuthConfig.AuthType = ConfigurationManager.AppSettings["AuthType"];
              //  AuthConfig.Environment = ConfigurationManager.AppSettings["Environment"];
            //}
        }

    }


}
