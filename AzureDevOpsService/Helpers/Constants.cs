using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsService.Helpers
{
    public class Constants
    {
        public const string ContentTypeJson = "application/json";
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "PUT";
        public const string Head = "HEAD";
        public const string SchemeBasic = "basic";
        public const string SchemeOAuth = "Bearer";
        public const string ContentTypeUriEncode = "application/x-www-form-urlencoded";

        public const string NotExists = "NotExists";
        public const string Exists = "Exists";
        public const string Success = "Success";
        public const string Failed = "Failed";
        public const string ProvisioningState = "succeeded";
        public const string SetException = "Failed";
        public const string Processing = "processing";
        public const string Exception = "exception";

        public const string ProductionEnvironment = "production";
        public const string StagingEnvironment = "staging";
        public const string DevEnvironment = "development";

        public const string BasicAuthentication = "pat";
        public const string TokenAuthentication = "token";


    }

    public class ConstResourceGroup
    {

        public const string NotFound = "resourcegroupnotfound";

    }

    public class OAuthConfigConsts
    {
        public  string TenantId = ConfigurationManager.AppSettings["TenantId"];
        public  string AuthorityUri = ConfigurationManager.AppSettings["AuthorityUri"];
        public  string ResourceUri = ConfigurationManager.AppSettings["ResourceUri"];
        public  string OAuthClientSecret = ConfigurationManager.AppSettings["OAuthClientSecret"];
        public  string GrantType = "authorization_code";

        public  string OAuthBaseUri = ConfigurationManager.AppSettings["OAuthBaseUri"];
        public  string OAuthParams = ConfigurationManager.AppSettings["OAuthParams"];
        public  string OAuthAudience = ConfigurationManager.AppSettings["OAuthAudience"];
        public  string OAuthClientId = ConfigurationManager.AppSettings["OAuthClientId"];
        public  string OAuthScope = ConfigurationManager.AppSettings["OAuthScope"];
        public  string OAuthRedirectionUri = ConfigurationManager.AppSettings["OAuthRedirectionUri"];
        public  string OAuthReponseState = ConfigurationManager.AppSettings["OAuthReponseState"];
        public  string OAuthReponseType = ConfigurationManager.AppSettings["OAuthReponseType"];
        public  string OAuthReponsePrompt = ConfigurationManager.AppSettings["OAuthReponsePrompt"];

        public  string AuthType = ConfigurationManager.AppSettings["OAuthReponsePrompt"];
        public  string Environment = ConfigurationManager.AppSettings["environment"];


    }
}
