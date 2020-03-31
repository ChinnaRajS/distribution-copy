using ExportWIAttachmentsWeb.Models;
using AzureDevOpsService.ApiService;
using AzureDevOpsService.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using distribution_copy.Controllers;
using distribution_copy.Services;
using distribution_copy.Models.AccountsResponse;
using distribution_copy.Models;

namespace ExportWIAttachmentsWeb.Controllers
{
    public class ExportWIAttachmentsController : Controller
    {
        StringBuilder logger = new StringBuilder();
        public string url = "";
        // GET: ExportWIAttachments
        public ActionResult Index()
        {
            AccountsResponse.AccountList accountList = new AccountsResponse.AccountList();
            try
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Session["PAT"])))
                {
                    string token = Convert.ToString(Session["PAT"]);
                    if (Session["AccountList"] != null)
                    {
                        accountList = (AccountsResponse.AccountList)Session["AccountList"];
                    }
                    else
                    {
                        RedirectToAction("../Account/Verify");
                    }

                    return View(accountList);
                }
                else
                {
                    return RedirectToAction("../Account/Verify");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult GetProjects(string accountName)
        {
            AccountDetail accountDetail = new AccountDetail();
            try
            {
                if (Convert.ToString(Session["PAT"]) != null)
                {
                    string token = Convert.ToString(Session["PAT"]);
                    ADOCLProjects projects = new ADOCLProjects(token);
                    ProjectDetail projectDetail = new ProjectDetail();
                    var getProjectResponse = projects.GetProjects(accountName);
                    projectDetail.ProjectDetails = new List<Value>();

                    projectDetail.ProjectDetails = JsonConvert.DeserializeObject<List<Value>>(JsonConvert.SerializeObject(getProjectResponse.ResponseAsDynamicObj.value));
                    return PartialView("_GetProjects", projectDetail);
                }
                else
                {
                    return RedirectToAction("../Account/Verify");
                }
            }
            catch (Exception)
            {
                return View(accountDetail);
            }
        }

        public ActionResult GetWorkItems(string accountName, string projectName)
        {
            AccountDetail accountDetail = new AccountDetail
            {
                WorkItemDetails = new List<WorkItemDetail>(),
                SelectedAccountName = accountName,
                SelectedProjectName = projectName
            };
            try
            {
                if (Convert.ToString(Session["PAT"]) != null)
                {
                    string token = Convert.ToString(Session["PAT"]);
                    CLWorkItem cLWorkItem = new CLWorkItem(token);

                    var workItems = cLWorkItem.GetWorkItemsByQuery(accountName, projectName);
                    if (workItems.Status)
                    {
                        var splitWI = cLWorkItem.WIAsList(workItems.ResponseAsDynamicObj.workItems);
                        foreach (var wi in splitWI)
                        {
                            var wiDetils = cLWorkItem.GetWIByIds(accountName, projectName, wi);
                            if (wiDetils.Status)
                            {
                                var wiDs = JsonConvert.DeserializeObject<AzureDevOpsService.Models.WorkItemFetchResponse.WorkItems>(wiDetils.ResponseAsString);
                                foreach (var widetail in wiDs.value)
                                {
                                    var wiDetailsMdel = new WorkItemDetail
                                    {
                                        Id = Convert.ToString(widetail.id),
                                        Name = Convert.ToString(widetail.fields.SystemTitle),
                                        Type = Convert.ToString(widetail.fields.SystemWorkItemType),
                                        AttachmentPath = new List<Attachment>()
                                    };
                                    if (widetail.relations != null)
                                    {
                                        foreach (var rel in widetail.relations)
                                        {
                                            if (rel != null)
                                            {
                                                if (rel.rel == "AttachedFile")
                                                {
                                                    Attachment attachment = new Attachment
                                                    {
                                                        Uri = rel.url
                                                    };
                                                    string attachmentUrl = rel.url;
                                                    int index = attachmentUrl.LastIndexOf("/");
                                                    string attachmentId = attachmentUrl.Substring(index + 1);
                                                    attachment.AttachmentId = attachmentId;
                                                    attachment.Name = rel.attributes["name"];
                                                    wiDetailsMdel.AttachmentPath.Add(attachment);
                                                }
                                            }
                                            //wiDetailsMdel.AttachmentPath.Add()
                                        }
                                    }
                                    accountDetail.WorkItemDetails.Add(wiDetailsMdel);
                                }
                            }
                        }
                    }
                    return PartialView("_GetWorkItems", accountDetail);
                }
                else
                {
                    return RedirectToAction("../Account/Verify");
                }
            }
            catch (Exception)
            {

                return View(accountDetail);
            }
        }

        public ActionResult ExportTestDetails(string accountName, string prjectName)
        {
            AccountDetail accountDetail = new AccountDetail();
            try
            {
                if (Convert.ToString(Session["PAT"]) != null)
                {
                    string token = Convert.ToString(Session["PAT"]);
                    ADOCLProjects projects = new ADOCLProjects(token);
                    ProjectDetail projectDetail = new ProjectDetail();
                    var getProjectResponse = projects.GetProjects(accountName);
                    projectDetail.ProjectDetails = new List<Value>();

                    projectDetail.ProjectDetails = JsonConvert.DeserializeObject<List<Value>>(JsonConvert.SerializeObject(getProjectResponse.ResponseAsDynamicObj.value));
                    return PartialView("_GetProjects", projectDetail);
                }
                else
                {
                    return RedirectToAction("../Account/Verify");
                }
            }
            catch (Exception)
            {

                return View(accountDetail);
            }
        }

        //[HttpPost]
        public ActionResult DownloadExcell(string accountName, string projectName)
        {
            CLTestCaseReport sample = new CLTestCaseReport();
            string token = Convert.ToString(Session["PAT"]);
            var vmProjects = sample.TestCaseDetails(token, accountName, projectName);
            using (ExcelPackage xp = new ExcelPackage())
            {
                string jso = JsonConvert.SerializeObject(vmProjects);
                int startRow = 2;

                var workSheet = xp.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                workSheet.Row(1).Height = 20;
                workSheet.Cells[1, 1, 1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thick);
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1].Value = "PlanName";
                workSheet.Cells[1, 2].Value = "Suite";
                workSheet.Cells[1, 3].Value = "TestCase";
                workSheet.Cells[1, 4].Value = "Steps";
                workSheet.Cells[1, 5].Value = "Expected Result";
                int startStyle = 2;
                //bool toggleColor = true;
                foreach (var plans in vmProjects.Plans)
                {
                    startStyle = startRow;
                    workSheet.Cells[startRow, 1].Value = plans.Name;
                    workSheet.Cells[startRow, 1].AutoFitColumns();// = true;
                    foreach (var suites in plans.Suites)
                    {
                        bool stepsExists = false;
                        workSheet.Cells[startRow, 2].Value = suites.Name;

                        workSheet.Cells[startRow, 2].AutoFitColumns();// = true;
                        foreach (var testcases in suites.TestCases)
                        {
                            workSheet.Cells[startRow, 3].Value = testcases.Name;

                            workSheet.Cells[startRow, 3].AutoFitColumns();// = true;
                            int i = 0;
                            foreach (var stepParams in testcases.Steps)
                            {
                                stepsExists = true;
                                bool isExpectedResult = false;
                                foreach (var stepItem in stepParams.StepNames)
                                {
                                    if (!isExpectedResult)
                                    {
                                        workSheet.Cells[startRow, 4].Value = "Step " + i + " : " + stepItem;
                                        workSheet.Cells[startRow, 4].AutoFitColumns();// = true;
                                        isExpectedResult = true;
                                        i++;
                                    }
                                    else
                                    {
                                        workSheet.Cells[startRow, 5].Value = stepItem;
                                        workSheet.Cells[startRow, 5].AutoFitColumns();// = true;
                                        isExpectedResult = false;
                                    }
                                }
                                if (stepParams.StepNames.Count > 0)
                                {
                                    startRow++;
                                }
                            }
                            startRow++;
                        }
                        if (!stepsExists)
                        {
                            startRow++;
                        }
                    }
                    startRow++;
                }
                HttpContext.Response.Clear();
                HttpContext.Response.AddHeader("", "");
                HttpContext.Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
                HttpContext.Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
                HttpContext.Response.AddHeader("content-disposition", "attachment;  filename=Report.xlsx");
                HttpContext.Response.ContentType = "application/text";
                HttpContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                HttpContext.Response.BinaryWrite(xp.GetAsByteArray());
                HttpContext.Response.End();
                return View();
            }
        }

        public ActionResult DownloadAttachments(string data)
        {
            Download model = JsonConvert.DeserializeObject<Download>(data);

       
            CreateZip.DirectoriesFiles sfiles = new CreateZip.DirectoriesFiles
            {
                Files = new List<CreateZip.FileInfo>(),
                Folder = new List<CreateZip.Folder>()
            };
            try
            {
                if (Convert.ToString(Session["PAT"]) != null)
                {
                    string token = Convert.ToString(Session["PAT"]);
                    CLWorkItem cLWorkItem = new CLWorkItem(token);
                    // the output bytes of the zip
                    byte[] fileBytes = null;
                    if (model.ExportType == "File")
                    {
                        foreach (var wi in model.DocumentIds)
                        {
                            CreateZip.FileInfo fileInfo = new CreateZip.FileInfo();
                            fileInfo.FileBytes = cLWorkItem.DownloadAttachment(model.AccountName, model.ProjectName, wi.DocId, wi.DocName);
                            String docName = wi.DocName;
                            int index = docName.LastIndexOf(".");
                            String documentName = docName.Substring(0, index);
                            String documentExtension = docName.Substring(index + 1);
                            fileInfo.Name = wi.WorkItemId + "__" + documentName;
                            fileInfo.Extension = documentExtension;
                            sfiles.Files.Add(fileInfo);
                        }

                        //create a working memory stream
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                        {
                            // create a zip
                            using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                            {
                                if (sfiles.Files != null && sfiles.Files.Count > 0)
                                {
                                    foreach (var outerFile in sfiles.Files)
                                    {
                                        // add the item name to the zip
                                        System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry(outerFile.Name + "." + outerFile.Extension);
                                        // add the item bytes to the zip entry by opening the original file and copying the bytes 
                                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(outerFile.FileBytes))
                                        {
                                            using (System.IO.Stream entryStream = zipItem.Open())
                                            {
                                                originalFileMemoryStream.CopyTo(entryStream);
                                            }
                                        }
                                    }
                                }
                            }
                            fileBytes = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        CreateZip.Folder folder = new CreateZip.Folder();
                        foreach (var wi in model.DocumentIds)
                        {
                            CreateZip.Folder folderq = new CreateZip.Folder();
                            folderq.FolderItems = new List<CreateZip.FolderItem>();

                            CreateZip.FolderItem folderItem = new CreateZip.FolderItem();
                            folderq.FolderName = wi.WorkItemId;
                            String fDocName = wi.DocName;
                            int fIndex = fDocName.LastIndexOf(".");
                            String folderItemName = fDocName.Substring(0, fIndex);
                            String folderItemExtension = fDocName.Substring(fIndex + 1);
                            folderItem.Name = folderItemName;
                            folderItem.Extension = folderItemExtension;
                            folderItem.FileBytes = cLWorkItem.DownloadAttachment(model.AccountName, model.ProjectName, wi.DocId, wi.DocName);
                            folderq.FolderItems.Add(folderItem);
                            sfiles.Folder.Add(folderq);

                        }

                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                        {
                            // create a zip
                            using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                            {
                                if (sfiles.Folder != null && sfiles.Folder.Count > 0)
                                {
                                    foreach (var fldr in sfiles.Folder)
                                    {
                                        // add the item name to the zip
                                        // each file in the folder
                                        foreach (var file in fldr.FolderItems)
                                        {
                                            // add the item name to the zip
                                            System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry(fldr.FolderName + "/" + file.Name + "." + file.Extension);
                                            // add the item bytes to the zip entry by opening the original file and copying the bytes 
                                            using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(file.FileBytes))
                                            {
                                                using (System.IO.Stream entryStream = zipItem.Open())
                                                {
                                                    originalFileMemoryStream.CopyTo(entryStream);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            fileBytes = memoryStream.ToArray();
                        }
                    }
                    // download the constructed zip
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "WIAttachments_" + model.ProjectName + ".zip");
                    return File(fileBytes, "application/zip");
                }
                else
                {
                    return RedirectToAction("../Account/Verify");
                }
            }
            catch (Exception ex)
            {
                logger.Append(ex.Message);
                logger.Append(ex.StackTrace);

                return RedirectToAction("../Account/Verify");
            }

        }
        public ActionResult DownloadExcel(string data)
        {
            distribution_copy.Models.ExpandWI.RootObject urlResponse = new distribution_copy.Models.ExpandWI.RootObject();
            Download model = new Download();
            model = JsonConvert.DeserializeObject<Download>(data);
            TraceInputModel input = new TraceInputModel();
            input.OrgName = model.AccountName;
            input.ProjectName = model.ProjectName;
            input.WIType = "Epic";
            AccountService accountService = new AccountService();
            TraceController trace = new TraceController();
            ExcelPackage excel = trace.TraceExport(input,false);
            var added = trace.added;
            urlResponse.value = ((distribution_copy.Models.ExpandWI.RootObject)System.Web.HttpContext.Current.Session["EWorkItems"]).value.Where(x=>(!added.Contains(x))&&x.fields.TeamProject==input.ProjectName).ToList();
            var workSheet = excel.Workbook.Worksheets[0];
            var colcount = workSheet.Dimension.End.Column;
            var rowCount = workSheet.Dimension.End.Row+1;
            List<string> colNames = new List<string>();
            for(int i = 1; i <= colcount; i++)
            {
             colNames.Add(workSheet.Cells[1, i].Value.ToString());
            }
            int titlecount = colNames.Where(x => x.ToLower().StartsWith("title")).Count();
            foreach (var WI in urlResponse.value)
            {
                int columnNo = 0;
                workSheet.Cells[rowCount, ++columnNo].Value = WI.id;
                workSheet.Cells[rowCount, ++columnNo].Value = WI.fields.WorkItemType;
                workSheet.Cells[rowCount, ++columnNo].Value = WI.fields.Title;
                columnNo += titlecount;
                workSheet.Cells[rowCount, columnNo++].Value = WI.fields.TeamProject;
                workSheet.Cells[rowCount, columnNo++].Value = WI.fields.State;
                workSheet.Cells[rowCount, columnNo++].Value = WI.fields.AreaPath;
                workSheet.Cells[rowCount, columnNo++].Value = WI.fields.IterationPath;
                rowCount++;
            }
            string excelName = input.OrgName + "-" + input.ProjectName + DateTime.Now.ToString();

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }


    }
}
