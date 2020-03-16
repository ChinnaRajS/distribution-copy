using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class TestPlanTestCaseFromSuit
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ProjectTestCaseFromSuit
    {
        public string id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

    public class TestSuite
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class WorkItemField
    {
        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.ActivatedBy")]
        public string __invalid_name__Microsoft_VSTS_Common_ActivatedBy { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.ActivatedDate")]
        public DateTime? __invalid_name__Microsoft_VSTS_Common_ActivatedDate { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.TCM.AutomationStatus")]
        public string __invalid_name__Microsoft_VSTS_TCM_AutomationStatus { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__System.State")]
        public string __invalid_name__System_State { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__System.AssignedTo")]
        public string __invalid_name__System_AssignedTo { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.Priority")]
        public int? __invalid_name__Microsoft_VSTS_Common_Priority { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime? __invalid_name__Microsoft_VSTS_Common_StateChangeDate { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__System.WorkItemType")]
        public string __invalid_name__System_WorkItemType { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__System.Rev")]
        public int? __invalid_name__System_Rev { get; set; }


    }

    public class WorkItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<WorkItemField> workItemFields { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class LinksTestCaseFromSuit
    {
        public Avatar avatar { get; set; }
    }

    public class Tester
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class PointAssignment
    {
        public int id { get; set; }
        public string configurationName { get; set; }
        public Tester tester { get; set; }
        public int configurationId { get; set; }
    }

    public class TestPointsTestCaseFromSuit
    {
        public string href { get; set; }
    }

    public class Configuration
    {
        public string href { get; set; }
    }

    public class SelfTestCaseFromSuit
    {
        public string href { get; set; }
    }

    public class SourcePlan
    {
        public string href { get; set; }
    }

    public class SourceSuite
    {
        public string href { get; set; }
    }

    public class SourceProject
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public TestPointsTestCaseFromSuit testPoints { get; set; }
        public Configuration configuration { get; set; }
        public Self _self { get; set; }
        public SourcePlan sourcePlan { get; set; }
        public SourceSuite sourceSuite { get; set; }
        public SourceProject sourceProject { get; set; }
    }

    public class ValueTestCaseFromSuit
    {
        public TestPlan testPlan { get; set; }
        public ProjectTestCaseFromSuit project { get; set; }
        public TestSuite testSuite { get; set; }
        public WorkItem workItem { get; set; }
        public List<PointAssignment> pointAssignments { get; set; }
        public Links2 links { get; set; }
    }

    public class TestCasesFromSuits
    {
        public List<ValueTestCaseFromSuit> value { get; set; }
        public int count { get; set; }
    }
}