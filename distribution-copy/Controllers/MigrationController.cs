using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using distribution_copy.Models.AccessDetails;
using distribution_copy.Models.AccountsResponse;
using distribution_copy.Models.InputModel;
using distribution_copy.Models.ProfileDetails;
using distribution_copy.Services;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using OfficeOpenXml;
using SharpCompress.Readers.Rar;
using WorkItemPublish;
using WorkItemPublish.Class;

namespace distribution_copy.Controllers
{
    public class MigrationController : Controller
    {
        static string URI = "";
        static string UserPAT = "";
        static string ProjectName = "";
        static public int titlecount = 0;
        static public List<string> titles = new List<string>();
        static DataTable DT;
        static List<string> TitleColumns = new List<string>();
        static public string OldTeamProject;
        static public string rarpath = "";

        readonly AccountService Account = new AccountService();

        // GET: Migration
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["visited"] == null)
            {
                return RedirectToAction("../Account/Verify");
            }
            if (Session["PAT"] == null)
            {
                try
                {
                    AccessDetails _accessDetails = new AccessDetails();
                    string code = Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString();
                    string redirectUrl = ConfigurationManager.AppSettings["RedirectUri"];
                    string clientId = ConfigurationManager.AppSettings["ClientSecret"];
                    string accessRequestBody = string.Empty;
                    accessRequestBody = Account.GenerateRequestPostData(clientId, code, redirectUrl);
                    _accessDetails = Account.GetAccessToken(accessRequestBody);
                    ProfileDetails profile = Account.GetProfile(_accessDetails);

                    if (!string.IsNullOrEmpty(_accessDetails.access_token))
                    {
                        Session["PAT"] = _accessDetails.access_token;

                        if (profile.displayName != null || profile.emailAddress != null)
                        {
                            Session["User"] = profile.displayName ?? string.Empty;
                            Session["Email"] = profile.emailAddress ?? profile.displayName.ToLower();
                        }
                    }
                }
                catch {  }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase Excel, HttpPostedFileBase Zip,string Org,string Proj)
        {
            URI= @"https://dev.azure.com/"+Org+"/";
            UserPAT = Session["PAT"] != null ? Session["PAT"].ToString() : "";
            ProjectName = Proj;
            try
            {
                var excelStream = Excel.InputStream;
                Stream zipStream;
                System.IO.Compression.ZipArchive zipArchive;
                List<WorkitemFromExcel> WiList;
                ExcelPackage excel = new ExcelPackage(excelStream);
                WIOps.ConnectWithPAT(URI, UserPAT);
                DT = ReadExcel(excel);
                if (Zip != null) { 
                zipStream = Zip.InputStream;
                zipArchive = new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Read);
                    WiList = GetWorkItems(zipArchive);
                }
                else
                    WiList = GetWorkItems();
                CreateLinks(WiList);
                ViewBag.message = "Migrated Succeffully";
            }
            catch(Exception ex)
            {
                //throw ex;
                ViewBag.message = "Something Went Wrong, Please Download Excel/Attachments From 'Export Attachments'";

            }
            return View();
        }
        static List<WorkitemFromExcel> GetWorkItems(System.IO.Compression.ZipArchive zipArchive=null)
        {
            AttatchmentAdder addAttachment = new AttatchmentAdder(URI,UserPAT);
            List<WorkitemFromExcel> workitemlist = new List<WorkitemFromExcel>();
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DataRow dr = DT.Rows[i];
                    string ID = dr["ID"].ToString();
                    if (!string.IsNullOrEmpty(ID))
                    {
                        try
                        {
                            WorkitemFromExcel item = new WorkitemFromExcel
                            {
                                Id = CreateWorkItem(dr)
                            };
                            if (zipArchive!=null)
                                addAttachment.FindAttachments(Convert.ToInt32(dr["ID"].ToString()), item.Id, zipArchive);
                            dr["ID"] = item.Id.ToString();
                            item.WiState = dr["State"].ToString();
                            item.AreaPath = dr["Area Path"].ToString();
                            item.Itertation = dr["Iteration Path"].ToString();
                            OldTeamProject = dr["Team Project"].ToString();
                            int columnindex = 0;
                            foreach (var col in TitleColumns)
                            {
                                if (!string.IsNullOrEmpty(col))
                                {
                                    if (!string.IsNullOrEmpty(dr[col].ToString()))
                                    {
                                        item.Title = dr[col].ToString();
                                        if (i > 0 && columnindex > 0)
                                            item.Parent = GetParentData(DT, i - 1, columnindex);
                                        break;
                                    }
                                }
                                columnindex++;
                            }
                            workitemlist.Add(item);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }

            return workitemlist;
        }
        static void CreateLinks(List<WorkitemFromExcel> WiList)
        {
            Dictionary<string, object> Fields;
            List<string> newStates = new List<string>() { "New", "To Do" };
            string Areapath;
            string iteration;
            foreach (var wi in WiList)
            {
                Fields = new Dictionary<string, object>();
                if (wi.Parent != null)
                    WIOps.UpdateWorkItemLink(wi.Parent.Id, wi.Id, "");
                if (!newStates.Contains(wi.WiState.ToString()))
                    Fields.Add("State", wi.WiState.ToString());
                Areapath = wi.AreaPath.ToString().Replace(OldTeamProject, ProjectName);
                Fields.Add("System.AreaPath", Areapath);
                iteration = wi.Itertation.ToString().Replace(OldTeamProject, ProjectName);
                Fields.Add("System.IterationPath", iteration);
                Fields.Add("System.TeamProject", ProjectName);
                WIOps.UpdateWorkItemFields(wi.Id, Fields);
            }
        }
         static ParentWorkItem GetParentData(DataTable dt, int rowindex, int columnindex)
        {
            ParentWorkItem workItem = new ParentWorkItem();

            if (columnindex > 0)
            {
                for (int i = rowindex; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    int colindex = columnindex;
                    while (colindex > 0)
                    {
                        int index = colindex - 1;
                        if (!string.IsNullOrEmpty(dr[TitleColumns[index]].ToString()))
                        {
                            workItem.Id = int.Parse(dr["ID"].ToString());
                            workItem.Title = dr[TitleColumns[index]].ToString();
                            break;
                        }
                        colindex--;
                    }
                    if (!string.IsNullOrEmpty(workItem.Title))
                    { break; }
                    /*if (hasParent == false)
                        return null;*/

                }
            }
            return workItem;

        }

        public static List<string> inavlidCoumns = new List<string>();
        static int CreateWorkItem(DataRow Dr)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            foreach (DataColumn column in DT.Columns)
            {
                if (Dr[column.ToString()].ToString() != "")
                {
                    if (column.ToString().StartsWith("Title"))
                        fields.Add("Title", Dr[column.ToString()]);
                    /*if (column.ToString()== "Work Item Type")
                    {          
                        fields.Add(column.ToString(), Dr[column.ToString()]);
                    }*/
                }
                if (fields.Count != 0)
                    break;
            }
            WorkItem newWi = new WorkItem();
            if (fields.Count != 0)
            {
                newWi = WIOps.CreateWorkItem(ProjectName, Dr["Work Item Type"].ToString(), fields);
            }
            return newWi.Id.Value;
        }

        public static DataTable ReadExcel(ExcelPackage Excel)
        {
            //Console.Write("Enter The Ecel File Path:");
            /*string ExcelPath=Console.ReadLine();*/
           var WorkSheet= Excel.Workbook.Worksheets[1];

            int rowCount = WorkSheet.Dimension.End.Row;
            int colCount = WorkSheet.Dimension.End.Column;
            DataTable Dt = new DataTable();
            DataRow row;
            for (int i = 1; i <= rowCount; i++)
            {
                row = Dt.NewRow();
                for (int j = 1; j <= colCount; j++)
                {
                    string ColName;
                    if (i == 1)
                    {
                        ColName = WorkSheet.Cells[i, j].Value.ToString();
                        if (ColName.StartsWith("Title"))
                        {
                            TitleColumns.Add(ColName);
                        }
                        DataColumn column = new DataColumn(ColName);
                        Dt.Columns.Add(column);
                    }
                    else
                    {
                        ColName = WorkSheet.Cells[1, j].Value.ToString();
                        if (WorkSheet.Cells[i, j].Value != null)
                            row[ColName] = WorkSheet.Cells[i, j].Value.ToString();
                    }
                }
                if (i != 1)
                    Dt.Rows.Add(row);
            }
            return Dt;
        }
    }
}