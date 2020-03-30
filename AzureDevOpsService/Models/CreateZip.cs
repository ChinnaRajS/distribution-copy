using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsService.Models
{
    public class CreateZip
    {
        public class FileInfo
        {
            public string Name { get; set; }
            public string Extension { get; set; }
            public Byte[] FileBytes { get; set; }
        }
        public class FolderItem
        {
            public string Name { get; set; }
            public string Extension { get; set; }
            public Byte[] FileBytes { get; set; }

        }
        public class Folder
        {
            public string FolderName { get; set; }
            public List<FolderItem> FolderItems { get; set; }
        }

        public class DirectoriesFiles
        {
            public List<FileInfo> Files { get; set; }
            public List<Folder> Folder { get; set; }
        }
    }
}
