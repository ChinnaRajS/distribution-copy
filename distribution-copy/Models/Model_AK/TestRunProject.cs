using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class ValueTest
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool isAutomated { get; set; }
        public string state { get; set; }
        public int totalTests { get; set; }
        public int incompleteTests { get; set; }
        public int notApplicableTests { get; set; }
        public int passedTests { get; set; }
        public int unanalyzedTests { get; set; }
        public int revision { get; set; }
        public string webAccessUrl { get; set; }
    }

    public class TestRunProject
    {
        public int count { get; set; }
        public List<ValueTest> value { get; set; }
    }

}