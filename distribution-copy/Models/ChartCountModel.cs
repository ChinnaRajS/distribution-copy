using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.ChartCountModel
{
    public class ChartCountModel
    {
        public Dictionary<string, Dictionary<string, int>> CountByProject { get; set; }

        public Dictionary<string, int> CountByOrg { get; set; }
        
    }
}