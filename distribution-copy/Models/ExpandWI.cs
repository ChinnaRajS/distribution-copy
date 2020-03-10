using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.ExpandWI
{
    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Avatar avatar { get; set; }
    }

    public class SystemCreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Avatar2
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public Avatar2 avatar { get; set; }
    }

    public class SystemChangedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links2 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Avatar3
    {
        public string href { get; set; }
    }

    public class Links3
    {
        public Avatar3 avatar { get; set; }
    }

    public class SystemAuthorizedAs
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links3 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Fields
    {
        [JsonProperty(PropertyName = "System.Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "System.AreaId")]
        public int AreaId { get; set; }

        [JsonProperty(PropertyName = "System.AreaPath")]
        public string AreaPath { get; set; }

        [JsonProperty(PropertyName = "System.TeamProject")]
        public string TeamProject { get; set; }

        [JsonProperty(PropertyName = "System.NodeName")]
        public string NodeName { get; set; }

        [JsonProperty(PropertyName = "System.AreaLevel1")]
        public string AreaLevel1 { get; set; }


        [JsonProperty(PropertyName = "System.Rev")]
        public int Rev { get; set; }

        [JsonProperty(PropertyName = "System.AuthorizedDate")]
        public DateTime AuthorizedDate { get; set; }

        [JsonProperty(PropertyName = "System.RevisedDate")]
        public DateTime RevisedDate { get; set; }

        [JsonProperty(PropertyName = "System.IterationId")]
        public int IterationId { get; set; }

        [JsonProperty(PropertyName = "System.IterationPath")]
        public string IterationPath { get; set; }

        [JsonProperty(PropertyName = "System.IterationLevel1")]
        public string IterationLevel1 { get; set; }

        [JsonProperty(PropertyName = "System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonProperty(PropertyName = "System.State")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "System.Reason")]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "System.Rev")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "System.CreatedBy")]
        public SystemCreatedBy CreatedBy { get; set; }

        [JsonProperty(PropertyName = "System.ChangedDate")]
        public DateTime ChangedDate { get; set; }

        [JsonProperty(PropertyName = "System.ChangedBy")]
        public SystemChangedBy ChangedBy { get; set; }

        [JsonProperty(PropertyName = "System.AuthorizedAs")]
        public SystemAuthorizedAs AuthorizedAs { get; set; }

        [JsonProperty(PropertyName = "System.PersonId")]
        public int PersonId { get; set; }

        [JsonProperty(PropertyName = "System.Watermark")]
        public int Watermark { get; set; }

        [JsonProperty(PropertyName = "System.CommentCount")]
        public int CommentCount { get; set; }

        [JsonProperty(PropertyName = "System.Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.RemainingWork")]
        public double RemainingWork { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime StateChangeDate { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
        public int Priority { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ValueArea")]
        public string ValueArea { get; set; }

        [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.Effort")]
        public double Effort { get; set; }

        [JsonProperty(PropertyName = "System.Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "System.Parent")]
        public int? Parent { get; set; }
    }

    public class Attributes
    {
        public bool isLocked { get; set; }
        public string comment { get; set; }
        public string name { get; set; }
    }

    public class Relation
    {
        public string rel { get; set; }
        public string url { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class WorkItemUpdates
    {
        public string href { get; set; }
    }

    public class WorkItemRevisions
    {
        public string href { get; set; }
    }

    public class WorkItemComments
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class WorkItemType
    {
        public string href { get; set; }
    }

    public class Fields2
    {
        public string href { get; set; }
    }

    public class Links4
    {
        public Self self { get; set; }
        public WorkItemUpdates workItemUpdates { get; set; }
        public WorkItemRevisions workItemRevisions { get; set; }
        public WorkItemComments workItemComments { get; set; }
        public Html html { get; set; }
        public WorkItemType workItemType { get; set; }
        public Fields2 fields { get; set; }
    }

    public class Value
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public List<Relation> relations { get; set; }
        public Links4 _links { get; set; }
        public string url { get; set; }
    }

    public class RootObject
    {
        public int count { get; set; }
        public List<Value> value { get; set; }
    }
}