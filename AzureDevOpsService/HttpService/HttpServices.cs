using AzureDevOpsService.Helpers;
using AzureDevOpsService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsService.HttpService
{
    public class HttpServices
    {
        private HttpConfigurations appConfig = new HttpConfigurations();
        public HttpServices(HttpConfigurations appConfigs)
        {
            appConfig.SecurityKey = appConfigs.SecurityKey;
            appConfig.BaseUrl = appConfigs.BaseUrl;
            appConfig.UrlParams = appConfigs.UrlParams;
            appConfig.ContentType = appConfigs.ContentType;
            appConfig.AuthenticationScheme = appConfigs.AuthenticationScheme;
            appConfig.HttpMethod = appConfigs.HttpMethod;
            appConfig.RequestBody = appConfigs.RequestBody;
            if (string.IsNullOrEmpty(appConfigs.AuthType))
            {
                appConfig.AuthType = ConfigurationManager.AppSettings["AuthType"];
            }
        }


        public ApiResponseMsg Get()
        {
            ApiResponseMsg respMsg = new ApiResponseMsg();
            respMsg.HttpResponseMsg = new HttpResponseMessage();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(appConfig.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(appConfig.ContentType));
                //if (appConfig.AuthType == Constants.SchemeBasic)
                //{
                //    appConfig.SecurityKey = Common.GetBase64Credentails(appConfig.SecurityKey);
                //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(appConfig.AuthenticationScheme, appConfig.SecurityKey);
                //}
                //else {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(appConfig.AuthType, appConfig.SecurityKey);
                //}

                if (appConfig.RequestBody != null)
                {
                    var jsonContent = new StringContent(appConfig.RequestBody, Encoding.UTF8, appConfig.ContentType);
                    var Request = new HttpRequestMessage(new HttpMethod(appConfig.HttpMethod), appConfig.UrlParams) { Content = jsonContent };

                    respMsg.HttpResponseMsg = client.SendAsync(Request).Result;
                }
                else

                {
                    respMsg.HttpResponseMsg = client.GetAsync(appConfig.UrlParams).Result;
                }
                if (respMsg.HttpResponseMsg.Content.Headers.ContentDisposition == null)
                {
                    respMsg.ResponseAsString = respMsg.HttpResponseMsg.Content.ReadAsStringAsync().Result;
                    respMsg.ResponseAsDynamicObj = Common.CreateDynamicModel(respMsg.ResponseAsString);
                    respMsg.Status = respMsg.HttpResponseMsg.IsSuccessStatusCode;
                    return respMsg;
                }
                else
                {
                    respMsg.ResponseAsString = respMsg.HttpResponseMsg.Content.ReadAsStringAsync().Result;

                    return respMsg;
                }
            }
        }

        public ApiResponseMsg Post(bool urlEncode = false, Dictionary<string, string> addtionalHeaders = null)
        {
            ApiResponseMsg responseMsg = new ApiResponseMsg();
            responseMsg.HttpResponseMsg = new HttpResponseMessage();



            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(appConfig.BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    if (addtionalHeaders != null)
                    {
                        foreach (var header in addtionalHeaders)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }

                    }
                    if (urlEncode)
                    {
                        var requestMsg = new HttpRequestMessage(HttpMethod.Post, appConfig.UrlParams);
                        requestMsg.Content = new StringContent(appConfig.RequestBody, Encoding.UTF8, appConfig.ContentType);
                        responseMsg.HttpResponseMsg = client.SendAsync(requestMsg).Result;
                    }
                    else
                    {

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(appConfig.ContentType));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(appConfig.AuthenticationScheme, appConfig.SecurityKey);
                        var jsonContent = new StringContent(appConfig.RequestBody, Encoding.UTF8, appConfig.ContentType);
                        var Request = new HttpRequestMessage(new HttpMethod(appConfig.HttpMethod), appConfig.UrlParams) { Content = jsonContent };
                        responseMsg.HttpResponseMsg = client.SendAsync(Request).Result;

                    }

                    responseMsg.ResponseAsString = responseMsg.HttpResponseMsg.Content.ReadAsStringAsync().Result;
                    responseMsg.Status = responseMsg.HttpResponseMsg.IsSuccessStatusCode;
                    responseMsg.ResponseAsDynamicObj = Common.CreateDynamicModel(responseMsg.ResponseAsString);
                    return responseMsg;

                }
            }
            catch (Exception ex)
            {
                responseMsg.Status = false;
                responseMsg.Exceptions = ex.StackTrace;
                return responseMsg;
            }
        }

        public ApiResponseMsg Put()
        {
            ApiResponseMsg responseMsg = new ApiResponseMsg();
            responseMsg.HttpResponseMsg = new HttpResponseMessage();
            try
            {

                using (var client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ContentTypeJson));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.SchemeOAuth, appConfig.SecurityKey);
                    var Method = new HttpMethod(Constants.Put);
                    var jsonContent = new StringContent(appConfig.RequestBody, Encoding.UTF8, Constants.ContentTypeJson);

                    var requestMsg = new HttpRequestMessage(Method, appConfig.BaseUrl + appConfig.UrlParams) { Content = jsonContent };

                    responseMsg.HttpResponseMsg = client.SendAsync(requestMsg).Result;

                    responseMsg.Status = responseMsg.HttpResponseMsg.IsSuccessStatusCode;
                    responseMsg.ResponseMsg = responseMsg.HttpResponseMsg.Content.ReadAsStringAsync().Result;
                    responseMsg.ResponseAsDynamicObj = Common.CreateDynamicModel(responseMsg.ResponseMsg);

                    return responseMsg;

                }
            }
            catch (Exception ex)
            {
                responseMsg.Status = false;
                responseMsg.Exceptions = ex.StackTrace;
                return responseMsg;
            }
        }

        public ApiResponseMsg Head()
        {
            ApiResponseMsg respMsg = new ApiResponseMsg();
            try
            {
                respMsg.HttpResponseMsg = new HttpResponseMessage();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(appConfig.BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(appConfig.ContentType));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(appConfig.AuthenticationScheme, appConfig.SecurityKey);

                    respMsg.HttpResponseMsg = client.GetAsync(appConfig.BaseUrl + appConfig.UrlParams).Result;
                    respMsg.ResponseAsString = respMsg.HttpResponseMsg.Content.ReadAsStringAsync().Result;
                    respMsg.ResponseAsDynamicObj = Common.CreateDynamicModel(respMsg.ResponseAsString);
                    respMsg.Status = respMsg.HttpResponseMsg.IsSuccessStatusCode;


                    return respMsg;
                }
            }
            catch (Exception ex)
            {
                respMsg.Status = false;
                respMsg.ErrorMessage = Constants.SetException;
                respMsg.Exceptions = ex.StackTrace;

                return respMsg;
            }
        }

    }
}
