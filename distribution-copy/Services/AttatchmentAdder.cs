using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkItemPublish
{
    class AttatchmentAdder
    {

        class RelConstants
        {
            public const string AttachmentRefStr = "AttachedFile";
        }
        static WorkItemTrackingHttpClient WitClient;
        public AttatchmentAdder(string _Url, string _PAT)
        {
            ConnectWithPAT(_Url, _PAT);
        }
        public void findAttachments(int oldId, int newId,System.IO.Compression.ZipArchive zipArchive)
        {
            foreach (var file in zipArchive.Entries.Where(x => x.FullName.StartsWith(oldId.ToString())))
                {
                    AddAttachment(newId, file);
                }            
        }
        static void AddAttachment(int WiID, System.IO.Compression.ZipArchiveEntry FilePath)
        {
            AttachmentReference att;

            using (var attStream=FilePath.Open())
            {
                att = WitClient.CreateAttachmentAsync(attStream, FilePath.Name).Result; // upload the file
            }
            List<object> references = new List<object>(); //list with references

            references.Add(new
            {
                rel = RelConstants.AttachmentRefStr,
                url = att.Url,
                attributes = new { comment = "" }
            });

            AddWorkItemRelations(WiID, references);
        }
        static WorkItem AddWorkItemRelations(int WIId, List<object> References)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            foreach (object rf in References)
                patchDocument.Add(new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = rf
                });

            return WitClient.UpdateWorkItemAsync(patchDocument, WIId).Result; // return updated work item
        }



        public static void ConnectWithPAT(string ServiceURL, string PAT)
        {
            VssConnection connection = new VssConnection(new Uri(ServiceURL), new VssOAuthAccessTokenCredential(PAT));
            InitClients(connection);
        }
        static void InitClients(VssConnection Connection)
        {
            WitClient = Connection.GetClient<WorkItemTrackingHttpClient>();
        }
    }


}
