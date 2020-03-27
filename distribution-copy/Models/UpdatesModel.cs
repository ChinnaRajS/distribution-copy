using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.UpdatesModel
{
    
        public class Avatar
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Avatar avatar { get; set; }
        }

        public class RevisedBy
        {
            public string id { get; set; }
            public string name { get; set; }
            public string displayName { get; set; }
            public string url { get; set; }
            public Links _links { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class SystemId
        {
            public int newValue { get; set; }
        }

        public class SystemAreaId
        {
            public int newValue { get; set; }
        }

        public class SystemNodeName
        {
            public string newValue { get; set; }
        }

        public class SystemAreaLevel1
        {
            public string newValue { get; set; }
        }

        public class SystemRev
        {
            public int newValue { get; set; }
            public int? oldValue { get; set; }
        }

        public class SystemAuthorizedDate
        {
            public DateTime newValue { get; set; }
            public DateTime? oldValue { get; set; }
        }

        public class SystemRevisedDate
        {
            public DateTime newValue { get; set; }
            public DateTime? oldValue { get; set; }
        }

        public class SystemIterationId
        {
            public int newValue { get; set; }
        }

        public class SystemIterationLevel1
        {
            public string newValue { get; set; }
        }

        public class SystemWorkItemType
        {
            public string newValue { get; set; }
        }

        public class SystemState
        {
            public string newValue { get; set; }
        }

        public class SystemReason
        {
            public string newValue { get; set; }
        }

        public class SystemAssignedTo
        {
        }

        public class SystemCreatedDate
        {
            public DateTime newValue { get; set; }
        }

        public class Avatar2
        {
            public string href { get; set; }
        }

        public class Links2
        {
            public Avatar2 avatar { get; set; }
        }

        public class NewValue
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links2 _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class SystemCreatedBy
        {
            public NewValue newValue { get; set; }
        }

        public class SystemChangedDate
        {
            public DateTime newValue { get; set; }
            public DateTime? oldValue { get; set; }
        }

        public class Avatar3
        {
            public string href { get; set; }
        }

        public class Links3
        {
            public Avatar3 avatar { get; set; }
        }

        public class NewValue2
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links3 _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class SystemChangedBy
        {
            public NewValue2 newValue { get; set; }
        }

        public class Avatar4
        {
            public string href { get; set; }
        }

        public class Links4
        {
            public Avatar4 avatar { get; set; }
        }

        public class NewValue3
        {
            public string displayName { get; set; }
            public string url { get; set; }
            public Links4 _links { get; set; }
            public string id { get; set; }
            public string uniqueName { get; set; }
            public string imageUrl { get; set; }
            public string descriptor { get; set; }
        }

        public class SystemAuthorizedAs
        {
            public NewValue3 newValue { get; set; }
        }

        public class SystemPersonId
        {
            public int newValue { get; set; }
        }

        public class SystemWatermark
        {
            public int newValue { get; set; }
            public int? oldValue { get; set; }
        }

        public class SystemIsDeleted
        {
            public bool newValue { get; set; }
        }

        public class SystemCommentCount
        {
            public int newValue { get; set; }
        }

        public class SystemTeamProject
        {
            public string newValue { get; set; }
        }

        public class SystemAreaPath
        {
            public string newValue { get; set; }
        }

        public class SystemIterationPath
        {
            public string newValue { get; set; }
        }

        public class SystemTitle
        {
            public string newValue { get; set; }
        }

        public class SystemBoardColumn
        {
            public string newValue { get; set; }
        }

        public class SystemBoardColumnDone
        {
            public bool newValue { get; set; }
        }

        public class SystemBoardLane
        {
            public string newValue { get; set; }
        }

        public class MicrosoftVSTSSchedulingRemainingWork
        {
            public double newValue { get; set; }
        }

        public class MicrosoftVSTSCommonStateChangeDate
        {
            public DateTime newValue { get; set; }
        }

        public class MicrosoftVSTSCommonPriority
        {
            public int newValue { get; set; }
        }

        public class MicrosoftVSTSCommonSeverity
        {
            public string newValue { get; set; }
        }

        public class MicrosoftVSTSCommonValueArea
        {
            public string newValue { get; set; }
        }

        public class MicrosoftVSTSSchedulingEffort
        {
            public double newValue { get; set; }
        }

        public class WEFB9597A544886406E9DD4C25E32B00056SystemExtensionMarker
        {
            public bool newValue { get; set; }
        }

        public class WEFB9597A544886406E9DD4C25E32B00056KanbanColumn
        {
            public string newValue { get; set; }
        }

        public class WEFB9597A544886406E9DD4C25E32B00056KanbanColumnDone
        {
            public bool newValue { get; set; }
        }

        public class WEFB9597A544886406E9DD4C25E32B00056KanbanLane
        {
            public string newValue { get; set; }
        }

        public class SystemDescription
        {
            public string newValue { get; set; }
        }

        public class Fields
        {
            [JsonProperty("System.Id")]
            public SystemId Id { get; set; }

            [JsonProperty("System.AreaId")]
            public SystemAreaId AreaId { get; set; }

            [JsonProperty("System.NodeName")]
            public SystemNodeName NodeName { get; set; }

            [JsonProperty("System.AreaLevel1")]
            public SystemAreaLevel1 AreaLevel1 { get; set; }

            [JsonProperty("System.Rev")]
            public SystemRev Rev { get; set; }

            [JsonProperty("System.AuthorizedDate")]
            public SystemAuthorizedDate AuthorizedDate { get; set; }

            [JsonProperty("System.RevisedDate")]
            public SystemRevisedDate RevisedDate { get; set; }

            [JsonProperty("System.IterationId")]
            public SystemIterationId IterationId { get; set; }

            [JsonProperty("System.IterationLevel1")]
            public SystemIterationLevel1 IterationLevel1 { get; set; }

            [JsonProperty("System.WorkItemType")]
            public SystemWorkItemType WorkItemType { get; set; }

            [JsonProperty("System.State")]
            public SystemState State { get; set; }

            [JsonProperty("System.Reason")]
            public SystemReason Reason { get; set; }

            [JsonProperty("System.AssignedTo")]
            public SystemAssignedTo AssignedTo { get; set; }

            [JsonProperty("System.CreatedDate")]
            public SystemCreatedDate CreatedDate { get; set; }

            [JsonProperty("System.CreatedBy")]
            public SystemCreatedBy CreatedBy { get; set; }

            [JsonProperty("System.ChangedDate")]
            public SystemChangedDate ChangedDate { get; set; }

            [JsonProperty("System.ChangedBy")]
            public SystemChangedBy ChangedBy { get; set; }

            [JsonProperty("System.AuthorizedAs")]
            public SystemAuthorizedAs AuthorizedAs { get; set; }

            [JsonProperty("System.PersonId")]
            public SystemPersonId PersonId { get; set; }

            [JsonProperty("System.Watermark")]
            public SystemWatermark Watermark { get; set; }

            [JsonProperty("System.IsDeleted")]
            public SystemIsDeleted IsDeleted { get; set; }

            [JsonProperty("System.CommentCount")]
            public SystemCommentCount CommentCount { get; set; }

            [JsonProperty("System.TeamProject")]
            public SystemTeamProject TeamProject { get; set; }

            [JsonProperty("System.AreaPath")]
            public SystemAreaPath AreaPath { get; set; }

            [JsonProperty("System.IterationPath")]
            public SystemIterationPath IterationPath { get; set; }

            [JsonProperty("System.Title")]
            public SystemTitle Title { get; set; }

            [JsonProperty("System.BoardColumn")]
            public SystemBoardColumn BoardColumn { get; set; }

            [JsonProperty("System.BoardColumnDone")]
            public SystemBoardColumnDone BoardColumnDone { get; set; }

            [JsonProperty("System.BoardLane")]
            public SystemBoardLane BoardLane { get; set; }

            [JsonProperty("Microsoft.VSTS.Scheduling.RemainingWork")]
            public MicrosoftVSTSSchedulingRemainingWork RemainingWork { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
            public MicrosoftVSTSCommonStateChangeDate StateChangeDate { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.Priority")]
            public MicrosoftVSTSCommonPriority Priority { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.Severity")]
            public MicrosoftVSTSCommonSeverity Severity { get; set; }

            [JsonProperty("Microsoft.VSTS.Common.ValueArea")]
            public MicrosoftVSTSCommonValueArea ValueArea { get; set; }

            [JsonProperty("Microsoft.VSTS.Scheduling.Effort")]
            public MicrosoftVSTSSchedulingEffort Effort { get; set; }

            [JsonProperty("WEF_B9597A544886406E9DD4C25E32B00056_System.ExtensionMarker")]
            public WEFB9597A544886406E9DD4C25E32B00056SystemExtensionMarker ExtensionMarker { get; set; }

            [JsonProperty("WEF_B9597A544886406E9DD4C25E32B00056_Kanban.Column")]
            public WEFB9597A544886406E9DD4C25E32B00056KanbanColumn Column { get; set; }

            [JsonProperty("WEF_B9597A544886406E9DD4C25E32B00056_Kanban.Column.Done")]
            public WEFB9597A544886406E9DD4C25E32B00056KanbanColumnDone Done { get; set; }

            [JsonProperty("WEF_B9597A544886406E9DD4C25E32B00056_Kanban.Lane")]
            public WEFB9597A544886406E9DD4C25E32B00056KanbanLane Lane { get; set; }

            [JsonProperty("System.Description")]
            public SystemDescription Description { get; set; }
        }

        public class Attributes
        {
            public DateTime authorizedDate { get; set; }
            public int id { get; set; }
            public DateTime resourceCreatedDate { get; set; }
            public DateTime resourceModifiedDate { get; set; }
            public DateTime revisedDate { get; set; }
            public string name { get; set; }
        }

        public class Added
        {
            public string rel { get; set; }
            public string url { get; set; }
            public Attributes attributes { get; set; }
        }

        public class Relations
        {
            public List<Added> added { get; set; }
        }

        public class Value
        {
            public int id { get; set; }
            public int workItemId { get; set; }
            public int rev { get; set; }
            public RevisedBy revisedBy { get; set; }
            public DateTime revisedDate { get; set; }
            public Fields fields { get; set; }
            public string url { get; set; }
            public Relations relations { get; set; }
        }

        public class RootObject
        {
            public int count { get; set; }
            public List<Value> value { get; set; }
        }
   
}