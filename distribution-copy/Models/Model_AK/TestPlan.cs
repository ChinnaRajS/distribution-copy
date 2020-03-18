using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

    public class RootSuite
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class ClientUrl
    {
        public string href { get; set; }
    }

    public class RootSuite2
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self _self { get; set; }
        public ClientUrl clientUrl { get; set; }
        public RootSuite2 rootSuite { get; set; }
    }

    public class ValuePlan
    {
        public int id { get; set; }
        public Project project { get; set; }
        public RootSuite rootSuite { get; set; }
        public Links _links { get; set; }
        public int revision { get; set; }
        public string name { get; set; }
        public string areaPath { get; set; }
        public string iteration { get; set; }
        public object owner { get; set; }
        public string state { get; set; }
    }

    public class TestPlan
    {
        public List<ValuePlan> value { get; set; }
        public int count { get; set; }

        public int id { get; set; }
        public Project project { get; set; }
        public RootSuite rootSuite { get; set; }
        public Links _links { get; set; }
        public int revision { get; set; }
        public string name { get; set; }
        public string areaPath { get; set; }
        public string iteration { get; set; }
        public object owner { get; set; }
        public string state { get; set; }
    }
}