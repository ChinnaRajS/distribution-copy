namespace AzureDevOpsService.Models
{
    public class OAuthConfiguration
    {
        public string AzureDevOpsBaseUrl { get; set; }
        public string AzureDevOpsParams { get; set; }
        public string AzureDevOpsClientId { get; set; }
        public string AzureDevOpsScope { get; set; }
        public string AzureDevOpsRedirectionUri { get; set; }
        public string AzureDevOpsReponseState { get; set; }
        public string AzureDevOpsReponseType { get; set; }
        public string AccessTokenParams { get; set; }
        public string AzureDevOpsAppSecret { get; set; }
        public string Environment { get; set; }
        public string TempScope { get; set; }
        public string AuthType { get; set; }

    }
}
