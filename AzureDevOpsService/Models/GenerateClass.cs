using System.Collections.Generic;

namespace AzureDevOpsService.Models
{


    public class Rootobject
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Subarea[] SubArea { get; set; }
    }

    public class Subarea
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public User[] Users { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Role[] Roles { get; set; }
    }

    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class MemberGroup
    {
        public string GroupName { get; set; }

        public string GroupDisplayName { get; set; }

        public string GroupPrincipalName { get; set; }

        public string TempAreaName { get; set; }
        public string SubAreaName { get; set; }

        public string AreaName { get; set; }

        public string Type { get; set; }


        public List<GroupMember> GroupMembers { get; set; }
    }

    public class GroupMember
    {
        public string MemberName { get; set; }
        public string MemberUserId { get; set; }

        public string Dev { get; set; }
        public string Test { get; set; }
        public string DevLead { get; set; }

        public string Manager { get; set; }

        public string TechLead { get; set; }
        public List<TempMemberGroup> TempGroups { get; set; }
        
    }
    public class TempMemberGroup
    {
        public string GroupName { get; set; }

    }

}
