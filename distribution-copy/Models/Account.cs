using AzureDevOpsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExportWIAttachmentsWeb.Models
{
    public class Account
    {
    }
    public class AccountDetail
    {
        public List<CLMAccount> AccountDetails { get; set; }
        public string SelectedAccountId { get; set; }

        public string SelectedAccountName { get; set; }
        public string SelectedProjectName { get; set; }

        public ProjectDetail ProjectDetails { get; set; }

        public List<WorkItemDetail> WorkItemDetails { get; set; }

    }

    public class ProjectDetail
    {
        public List<Value> ProjectDetails { get; set; }
        public string SelectedAccountId { get; set; }
    }

    public class WorkItemDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Attachment> AttachmentPath { get; set; }

    }
    public class Attachment
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string AttachmentId { get; set; }
    }

    public class Download
    {
        public string AccountName { get; set; }
        public string ProjectName { get; set; }
        public string ExportType { get; set; }
        public List<DocumentsModel> DocumentIds { get; set; }
    }

    public class DocumentsModel
    {
        public string WorkItemId { get; set; }
        public string DocId { get; set; }
        public string DocName { get; set; }
    }


}