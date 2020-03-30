using System;

namespace AzureDevOpsService.Models
{

    public class Properties
    {
    }

    public class CLMAccount
    {
        public string AccountId { get; set; }
        public string NamespaceId { get; set; }
        public string AccountName { get; set; }
        public object OrganizationName { get; set; }
        public int AccountType { get; set; }
        public string AccountOwner { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AccountStatus { get; set; }
        public object StatusReason { get; set; }
        public string LastUpdatedBy { get; set; }
        public Properties Properties { get; set; }
    }

}
