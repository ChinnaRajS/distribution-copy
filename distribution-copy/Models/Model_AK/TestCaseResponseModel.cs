using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class AvatarTestCase
    {
        public string href { get; set; }
    }

    public class LinksTestCase
    {
        public Avatar avatar { get; set; }
    }

    public class SystemAssignedToTestCase
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Avatar2TestCaseResponse
    {
        public string href { get; set; }
    }

    public class Links2TestCase
    {
        public Avatar2 avatar { get; set; }
    }

    public class SystemCreatedByTestCase
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links2 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Avatar3TestCase
    {
        public string href { get; set; }
    }

    public class Links3TestCase
    {
        public Avatar3 avatar { get; set; }
    }

    public class SystemChangedByTestCase
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links3TestCase _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Avatar4TestCase
    {
        public string href { get; set; }
    }

    public class Links4TestCase
    {
        public Avatar4 avatar { get; set; }
    }

    public class MicrosoftVSTSCommonActivatedByTestCase
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links4 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class FieldsTestCase
    {
        [JsonProperty(PropertyName = "__invalid_name__System.AreaPath")]
        public string AreaPath { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.TeamProject")]
        public string TeamProject { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.IterationPath")]
        public string IterationPath { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.WorkItemType")]
        public string WorkItemType { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.State")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.Reason")]
        public string Reason { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.AssignedTo")]
        public SystemAssignedTo AssignedTo { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.CreatedBy")]
        public SystemCreatedBy CreatedBy { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.ChangedDate")]
        public DateTime ChangedDate { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.ChangedBy")]
        public SystemChangedBy ChangedBy { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.CommentCount")]
        public int CommentCount { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__System.Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime StateChangeDate { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.ActivatedDate")]
        public DateTime ActivatedDate { get; set; }

        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.ActivatedBy")]
        public MicrosoftVSTSCommonActivatedBy ActivatedBy { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.Common.Priority")]
        public int Priority { get; set; }
        [JsonProperty(PropertyName = "__invalid_name__Microsoft.VSTS.TCM.AutomationStatus")]
        public string AutomationStatus { get; set; }
    }

    public class SelfTestCase
    {
        public string href { get; set; }
    }

    public class WorkItemUpdatesTestCase
    {
        public string href { get; set; }
    }

    public class WorkItemRevisionsTestCase
    {
        public string href { get; set; }
    }

    public class WorkItemCommentsTestCase
    {
        public string href { get; set; }
    }

    public class HtmlTestCase
    {
        public string href { get; set; }
    }

    public class WorkItemTypeTestCase
    {
        public string href { get; set; }
    }

    public class Fields2TestCase
    {
        public string href { get; set; }
    }

    public class Links5TestCase
    {
        public Self self { get; set; }
        public WorkItemUpdates workItemUpdates { get; set; }
        public WorkItemRevisions workItemRevisions { get; set; }
        public WorkItemComments workItemComments { get; set; }
        public Html html { get; set; }
        public WorkItemType workItemType { get; set; }
        public Fields2 fields { get; set; }
    }

    public class TestCaseResponseModel
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public Links5 _links { get; set; }
        public string url { get; set; }
    }
}