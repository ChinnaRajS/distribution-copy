using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace distribution_copy.Models.Model_AK
{
    public class CheckValid
    {
        public static bool TestPlanValid { get; set; }
        public static bool TestSuitValid { get; set; }
        public static bool TestCaseValid { get; set; }

        public static bool AddTestcase { get; set; }

    }
}