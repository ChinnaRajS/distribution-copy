using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class ProjectSuit
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

    public class Plan
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class SelfSuit
    {
        public string href { get; set; }
    }

    public class TestCases
    {
        public string href { get; set; }
    }

    public class TestPoints
    {
        public string href { get; set; }
    }

    public class LinksSuit
    {
        public Self _self { get; set; }
        public TestCases testCases { get; set; }
        public TestPoints testPoints { get; set; }
    }

    public class DefaultConfiguration
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ParentSuite
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ValueSuit
    {
        public int id { get; set; }
        public int revision { get; set; }
        public Project project { get; set; }
        public DateTime lastUpdatedDate { get; set; }
        public Plan plan { get; set; }
        public bool hasChildren { get; set; }
        public Links _links { get; set; }
        public string suiteType { get; set; }
        public string name { get; set; }
        public bool inheritDefaultConfigurations { get; set; }
        public List<DefaultConfiguration> defaultConfigurations { get; set; }
        public ParentSuite parentSuite { get; set; }
    }

    public class TestSuit
    {
        public List<ValueSuit> value { get; set; }
        public int count { get; set; }
    }

}