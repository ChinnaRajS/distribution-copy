using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureDevOpsService.Models
{

    public class VMRootTestCase
    {
        public List<VMProject> Projects { get; set; }
    }

    public class VMProject
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<VMPlan> Plans { get; set; }
    }

    public class VMPlan
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<VMSuite> Suites { get; set; }
    }

    public class VMSuite
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<VMTestcase> TestCases { get; set; }
    }

    public class VMTestcase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Step> Steps { get; set; }
    }

    public class Step
    {
        public string Id { get; set; }
        public List<string> StepNames { get; set; }

    }

}
