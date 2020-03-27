using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsService.Models
{
    public class ApiResponseMsg
    {
        public string Message { get; set; }
        public HttpResponseMessage HttpResponseMsg { get; set; }
        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string Exceptions { get; set; }
        public string ResponseMsg { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseAsString { get; set; }
        public dynamic ResponseAsDynamicObj { get; set; }
        public string CurrentStatus { get; set; }
    }
}
