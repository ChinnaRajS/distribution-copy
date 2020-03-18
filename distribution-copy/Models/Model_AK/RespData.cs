using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class ValueResp
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }
    }

    public class RespData
    {
        public static int count { get; set; }
        public List<ValueResp> value { get; set; }

     //   public List<WorkItemCount> workitemcount { get; set; }
    }
}