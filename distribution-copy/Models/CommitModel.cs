using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.CommitModel
{
    public class Author
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime date { get; set; }
        public string imageUrl { get; set; }
    }

    public class Committer
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime date { get; set; }
        public string imageUrl { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Repository
    {
        public string href { get; set; }
    }

    public class Web
    {
        public string href { get; set; }
    }

    public class Changes
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Repository repository { get; set; }
        public Web web { get; set; }
        public Changes changes { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links2
    {
        public Avatar avatar { get; set; }
    }

    public class PushedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links2 _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Push
    {
        public PushedBy pushedBy { get; set; }
        public int pushId { get; set; }
        public DateTime date { get; set; }
    }

    public class CommitModel
    {
        public string treeId { get; set; }
        public string commitId { get; set; }
        public Author author { get; set; }
        public Committer committer { get; set; }
        public string comment { get; set; }
        public List<string> parents { get; set; }
        public string url { get; set; }
        public string remoteUrl { get; set; }
        public Links _links { get; set; }
        public Push push { get; set; }
    }
}