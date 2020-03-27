using System;

namespace AzureDevOpsService.Models
{

    public class AreaClassification
    {
        public int count { get; set; }
        public AreaNodes[] value { get; set; }
    }

    public class AreaNodes
    {
        public int id { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public string structureType { get; set; }
        public bool hasChildren { get; set; }
        public Child[] children { get; set; }
        public string path { get; set; }
        public string url { get; set; }
    }

    public class Child
    {
        public int id { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public string structureType { get; set; }
        public bool hasChildren { get; set; }
        public Child1[] children { get; set; }
        public string path { get; set; }
        public string url { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Attributes
    {
        public DateTime startDate { get; set; }
        public DateTime finishDate { get; set; }
    }

    public class Child1
    {
        public int id { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public string structureType { get; set; }
        public bool hasChildren { get; set; }
        public string path { get; set; }
        public string url { get; set; }
    }

}
