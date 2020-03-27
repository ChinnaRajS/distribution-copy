using Newtonsoft.Json;

namespace AzureDevOpsService.Models
{
    public class CLTestCase
    {
        public class CLTestCaseDetail
        {
            public int count { get; set; }
            public Value[] value { get; set; }
        }

        public class Value
        {
            public int id { get; set; }
            public int rev { get; set; }
            public Fields fields { get; set; }
        }

        public class Fields
        {
            [JsonProperty(PropertyName = "System.Title")]
            public string Title { get; set; }
            [JsonProperty(PropertyName = "System.AreaPath")]
            public string AreaPath { get; set; }
            [JsonProperty(PropertyName = "System.TeamProject")]
            public string TeamProject { get; set; }
            [JsonProperty(PropertyName = "System.IterationPath")]
            public string IterationPath { get; set; }
            [JsonProperty(PropertyName = "System.WorkItemType")]
            public string WorkItemType { get; set; }
            [JsonProperty(PropertyName = "System.State")]
            public string SystemState { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.TCM.Steps")]
            public string Steps { get; set; }
        }

        
    }
    public class CLSteps
    {
        public class CLStepsTemplate
        {
            public RootStep Steps { get; set; }
        }
            public class RootStep
        {
            public string id { get; set; }
            public string last { get; set; }
            public StepDetails[] step { get; set; }
        }

        public class StepDetails
        {
            public string id { get; set; }
            public string type { get; set; }
            public Parameterizedstring[] parameterizedString { get; set; }
            public object description { get; set; }
        }

        public class Parameterizedstring
        {
            public string isformatted { get; set; }
            public string text { get; set; }
        }


    }
}
