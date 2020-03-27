using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsService.Models
{
   public class CLMProfile
    {
            public string displayName { get; set; }
            public string publicAlias { get; set; }
            public string emailAddress { get; set; }
            public int coreRevision { get; set; }
            public DateTime timeStamp { get; set; }
            public string id { get; set; }
            public int revision { get; set; }
    }
}
