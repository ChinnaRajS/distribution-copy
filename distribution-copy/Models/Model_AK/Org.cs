using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class Org
    {
        public static string OrganizationName { get; set; }
        public RespData ProjectData { get; set; }

        public IList<ProjectList> ProjectListData { get; set; }
        public IList<string> ProjectListData1 { get; set; }

        public static string token = "deuhz62cyulysixhr4dudqjooh5r3vd7mjbuqc256ywmk5qho7bq";
        //string token = "gwdvya3p7kbkpzskdthiae5qmm7qavyqo56gebj5b2senwzh35va";
        // public string pat = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", token)));
        public static string pat { get; set; }
        public static string ProjectName { get; set; }

        public RespData ProjectNameList { get; set; }

  //      public IEnumerable<SelectListItem> DropdownList { get; set; }
        public bool hasmodel { get; set; }


    }
    public class JsonTestCase
    {
        public static string JsonTestCaseResponse { get; set; }
    }
    public class ProjectList
    {
        public string ProjectName { get; set; }
//        public List<WorkItemCount> WorkItemList { get; set; }
    }
}
