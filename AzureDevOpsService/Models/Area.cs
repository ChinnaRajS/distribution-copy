using System.Collections.Generic;

namespace AzureDevOpsService.Models
{
    public class Area
    {
        public class RootArea
        {
            public List<AreaDetail> Areas { get; set; }

        }

        public class AreaDetail
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Type { get; set; }
            public List<Subarea> SubArea { get; set; }
            public List<User> Users { get; set; }
        }

        public class Subarea
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public List<User> Users { get; set; }
        }

        public class User
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public List<Role> Roles { get; set; }
        }

        public class Role
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

    }
}
