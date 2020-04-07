using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.ResponseWIAPI
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
        public class Fields
        {
            [JsonProperty(PropertyName = "System.AreaPath")]
            public string AreaPath { get; set; }

            [JsonProperty(PropertyName = "System.TeamProjecty")]
            public string TeamProject { get; set; }

            [JsonProperty(PropertyName = "System.IterationPath")]
            public string IterationPath { get; set; }

            [JsonProperty(PropertyName = "System.WorkItemType")]
            public string WorkItemType { get; set; }

            [JsonProperty(PropertyName = "System.State")]
            public string State { get; set; }

            [JsonProperty(PropertyName = "System.Reason")]
            public string Reason { get; set; }

            [JsonProperty(PropertyName = "System.CreatedDate")]
            public DateTime CreatedDate { get; set; }

            [JsonProperty(PropertyName = "System.CreatedBy")]
            public SystemCreatedBy CreatedBy { get; set; }

            [JsonProperty(PropertyName = "System.ChangedDate")]
            public DateTime ChangedDate { get; set; }

            [JsonProperty(PropertyName = "System.ChangedBy")]
            public SystemChangedBy ChangedBy { get; set; }

            [JsonProperty(PropertyName = "System.CommentCount")]
            public int CommentCount { get; set; }

            [JsonProperty(PropertyName = "System.Title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "System.BoardColumn")]
            public string BoardColumn { get; set; }

            [JsonProperty(PropertyName = "System.BoardColumnDone")]
            public bool BoardColumnDone { get; set; }

            [JsonProperty(PropertyName = "System.BoardLane")]
            public string BoardLane { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.RemainingWork")]
            public double RemainingWork { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.Common.StateChangeDate")]
            public DateTime StateChangeDate { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
            public int Priority { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Severity")]
            public string Severity { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ValueArea")]
            public string ValueArea { get; set; }

            [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.Effort")]
            public double Effort { get; set; }

            [JsonProperty(PropertyName = "System.Description")]
            public string Description { get; set; }

        }
        public class Value
        {
            public int id { get; set; }
            public int rev { get; set; }
            public Fields fields { get; set; }
            public string url { get; set; }
        }

        public class ResponseWIAPI
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }
 }
