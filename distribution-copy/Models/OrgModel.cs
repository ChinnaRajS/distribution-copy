
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using distribution_copy.Models.ProjectModel;

namespace distribution_copy.Models.OrgModel
{
    public class OrgModel
    {
        public int Count { get; set; }
        public List<ProjectDetails> Value { get; set; }
        public orgCounts counts { get; set; }
    }
    public class countGen
    {
        public int Count { get; set; }
    }
    public class orgCounts
    {
        public int buildDefCount { get; set; }
        public int releaseDefCount { get; set; }
        public int repoCount { get; set; }
        public int UserCount { get; set; }
        public int processCount { get; set; }
        public int WIcountOrg { get; set; }
        public int WIcountType { get; set; }
        public int ProjWIcountByType { get; set; }

    }

    public class MembersMod
    {
        public List<Member> members { get; set; }
        public string continuationToken { get; set; }
        public int totalCount { get; set; }
    }
    public class Member
    {
        public string id { get; set; }

    }


}