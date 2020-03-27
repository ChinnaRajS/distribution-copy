using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class ConfigurationTestRunById
    {
    public string id { get; set; }
    public string name { get; set; }
}

public class ProjectTestRunById
    {
    public string id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
}

public class TestCase
{
    public string id { get; set; }
    public string name { get; set; }
}

public class TestPoint
{
    public string id { get; set; }
}

public class TestRun
{
    public string id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
}

public class TestPlanTestRunById
    {
    public string id { get; set; }
}

public class AvatarTestRunById
    {
    public string href { get; set; }
}

public class LinksTestRunById
    {
    public Avatar avatar { get; set; }
}

public class Owner
{
    public string displayName { get; set; }
    public string url { get; set; }
    public Links _links { get; set; }
    public string id { get; set; }
    public string uniqueName { get; set; }
    public string imageUrl { get; set; }
    public string descriptor { get; set; }
}

public class Avatar2
{
    public string href { get; set; }
}

public class Links2TestRunById
    {
    public Avatar2 avatar { get; set; }
}

public class RunBy
{
    public string displayName { get; set; }
    public string url { get; set; }
    public Links2 _links { get; set; }
    public string id { get; set; }
    public string uniqueName { get; set; }
    public string imageUrl { get; set; }
    public string descriptor { get; set; }
}

public class Avatar3
{
    public string href { get; set; }
}

public class Links3
{
    public Avatar3 avatar { get; set; }
}

public class LastUpdatedBy
{
    public string displayName { get; set; }
    public string url { get; set; }
    public Links3 _links { get; set; }
    public string id { get; set; }
    public string uniqueName { get; set; }
    public string imageUrl { get; set; }
    public string descriptor { get; set; }
}

public class ValueTestRunById
    {
    public int id { get; set; }
    public Configuration configuration { get; set; }
    public Project project { get; set; }
    public DateTime startedDate { get; set; }
    public DateTime completedDate { get; set; }
    public double durationInMs { get; set; }
    public string outcome { get; set; }
    public int revision { get; set; }
    public string state { get; set; }
    public TestCase testCase { get; set; }
    public TestPoint testPoint { get; set; }
    public TestRun testRun { get; set; }
    public DateTime lastUpdatedDate { get; set; }
    public int priority { get; set; }
    public DateTime createdDate { get; set; }
    public string url { get; set; }
    public string failureType { get; set; }
    public string testCaseTitle { get; set; }
    public int testCaseRevision { get; set; }
    public List<object> customFields { get; set; }
    public TestPlan testPlan { get; set; }
    public int testCaseReferenceId { get; set; }
    public Owner owner { get; set; }
    public RunBy runBy { get; set; }
    public LastUpdatedBy lastUpdatedBy { get; set; }
}

public class TestRunById
    {
    public int count { get; set; }
    public List<ValueTestRunById> value { get; set; }
}

}