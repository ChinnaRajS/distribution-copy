using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using distribution_copy.Models.ExpandWI;

namespace distribution_copy.Models.TraceExportModel
{
    public class TraceExportModel
    {
        public RootObject ParentWI { get; set; }
        public List<TraceExportModel> ChildWI { get; set; }
    }
}