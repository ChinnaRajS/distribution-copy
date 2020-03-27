using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
 
        public class AvatarWorkItemsClass
        {
            public string href { get; set; }
        }

        public class LinksWorkItemsClass
        {
            public Avatar avatar { get; set; }
        }

        public class SystemAssignedTo
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class Avatar2WorkItemsClass
        {
            public string href { get; set; }
        }

        public class Links2WorkItemsClass
        {
            public Avatar2 avatar { get; set; }
        }

        public class SystemCreatedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links2 _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class Avatar3WorkItem
        {
            public string href { get; set; }
        }

        public class Links3WorkItemsClass
        {
            public Avatar3 avatar { get; set; }
        }

        public class SystemChangedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links3WorkItemsClass _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class Avatar4
        {
            public string href { get; set; }
        }

        public class Links4
        {
            public Avatar4 avatar { get; set; }
        }

        public class MicrosoftVSTSCommonActivatedBy
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links4 _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class Fields
        {

            [JsonProperty(PropertyName = "System.AreaPath")]
            public string AreaPath { get; set; }
            //public string __invalid_name__System.TeamProject { get; set; }
            //public string __invalid_name__System.IterationPath { get; set; }
            [JsonProperty(PropertyName = "WorkItemType")]
            public string WorkItemType { get; set; }
            [JsonProperty(PropertyName = "System.State")]
            public string State { get; set; }
            // public string __invalid_name__System.Reason { get; set; }

            //[JsonProperty(PropertyName = "System.AssignedTo")]
            //public SystemAssignedTo AssignedTo { get; set; }
            //public DateTime __invalid_name__System.CreatedDate { get; set; }

            [JsonProperty(PropertyName = "System.CreatedBy")]
            public SystemCreatedBy CreatedBy { get; set; }
            //public DateTime __invalid_name__System.ChangedDate { get; set; }
            //public SystemChangedBy __invalid_name__System.ChangedBy { get; set; }
            //public int __invalid_name__System.CommentCount { get; set; }

            [JsonProperty(PropertyName = "System.Title")]
            public string Title { get; set; }
            //public DateTime __invalid_name__Microsoft.VSTS.Common.StateChangeDate { get; set; }
            //public DateTime __invalid_name__Microsoft.VSTS.Common.ActivatedDate { get; set; }
            //public MicrosoftVSTSCommonActivatedBy __invalid_name__Microsoft.VSTS.Common.ActivatedBy { get; set; }
            [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
            public int Priority { get; set; }
            //public string __invalid_name__Microsoft.VSTS.TCM.AutomationStatus { get; set; }
        }

        public class SelfWorkItemsClass
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

        public class Links5
        {
            public Self self { get; set; }
            public WorkItemUpdates workItemUpdates { get; set; }
            public WorkItemRevisions workItemRevisions { get; set; }
            public WorkItemComments workItemComments { get; set; }
            public Html html { get; set; }
            public WorkItemType workItemType { get; set; }
            public Fields2 fields { get; set; }
        }

        public class WorkItemsClass
        {
            public int id { get; set; }
            public int rev { get; set; }
            public Fields fields { get; set; }
            public Links5 _links { get; set; }
            public string url { get; set; }
        }
    }
