using distribution_copy.Models.AccessDetails;
using distribution_copy.Models.AccountsResponse;
using distribution_copy.Models.InputModel;
using distribution_copy.Models.ProfileDetails;
using distribution_copy.Models.ExpandWI;
using distribution_copy.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using distribution_copy.Models.ResponseWI;
using distribution_copy.Models.TraceExportModel;
using distribution_copy.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace distribution_copy.Controllers
{
    public class TraceController : Controller
    {
        public string url = "";

        AccountService Account = new AccountService();
        ExcelPackage excel = new ExcelPackage();
        ExcelWorksheet workSheet;
        int recordIndex ;
        int columnNo;
        int i = 0;

        // GET: Trace
        public ActionResult Index()
        {
            if (Session["visited"] == null)
            {
                return RedirectToAction("../Account/Verify");
            }
            if (Session["PAT"] == null)
            {
                InputModel input = new InputModel();
                try
                {

                    AccessDetails _accessDetails = new AccessDetails();

                    AccountsResponse.AccountList accountList = null;
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
                    accountList = Account.GetAccounts(profile.id, _accessDetails);
                    Session["AccountList"] = accountList;
                    string pat = Session["PAT"].ToString();
                    List<SelectListItem> OrganizationList = new List<SelectListItem>();
                    foreach (var i in accountList.value)
                    {
                        OrganizationList.Add(new SelectListItem { Text = i.accountName, Value = i.accountName });
                    }
                    ViewBag.OrganizationList = OrganizationList;



                }
                catch (Exception ex)
                {



                }
            }
            return View();
        }
        [HttpPost]
        public JsonResult WITypes(InputModel inp)
        {
            ResponseWI wiqlResponse = new ResponseWI();

            RootObject urlResponse = new RootObject();
            string responseBody = "";
            string queryString = @"Select [Work Item Type],[State], [Title],[Created By] From WorkItems ";

            queryString += "Order By [Stack Rank] Desc, [Backlog Priority] Desc";
            var wiql = new
            {
                query = queryString
            };
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(wiql), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString()))));
                var request = new HttpRequestMessage(
                    new HttpMethod("POST"),
                    "https://dev.azure.com/" + inp.OrganizationName + "/_apis/wit/wiql?api-version=5.1"
                    )
                { Content = content };
                var response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    responseBody = response.Content.ReadAsStringAsync().Result;
                }
                wiqlResponse = JsonConvert.DeserializeObject<ResponseWI>(responseBody);
            }

            if (wiqlResponse.workItems == null || wiqlResponse.workItems.Count == 0)
                return null;
            string defaultUrl = "https://dev.azure.com/" + inp.OrganizationName + "/_apis/wit/workitems?ids=";
            url = defaultUrl;
            urlResponse.value = new List<distribution_copy.Models.ExpandWI.Value>();
            string b = "&$expand=all&api-version=5.1";
            for (int j = 0; j < wiqlResponse.workItems.Count; j++)
            {
                if (j % 200 == 0 && j != 0)
                {

                    var batchResponse = getWorkItems(inp);
                    urlResponse.count += batchResponse.count;
                    foreach (var item in batchResponse.value)
                    {
                        urlResponse.value.Add(item);
                    }
                    url = defaultUrl;
                }
                if (j % 200 == 0)
                {
                    url += wiqlResponse.workItems[j].id;
                }
                else
                {
                    url += "," + wiqlResponse.workItems[j].id;
                }
            }
            url += b;

            var lastBatchResponse = getWorkItems(inp);
            urlResponse.count += lastBatchResponse.count;
            foreach (var item in lastBatchResponse.value)
            {
                urlResponse.value.Add(item);
            }
            Session["EWorkItems"] = urlResponse;
            List<string> Types = new List<string>();
            foreach (var i in urlResponse.value)
            {
                if (!Types.Contains(i.fields.WorkItemType))
                    Types.Add(i.fields.WorkItemType);
            }
            return Json(Types, JsonRequestBehavior.AllowGet);
        }
        public RootObject getWorkItems(InputModel inp)
        {
            RootObject batch;
            string responseBody2 = "";
            url += "&$expand=all&api-version=5.1";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString()))));

                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        responseBody2 += response.Content.ReadAsStringAsync().Result;
                        batch = JsonConvert.DeserializeObject<RootObject>(responseBody2);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return batch;

        }
        public object Filter(InputModel inp, int loc)
        {

            RootObject wI = (RootObject)Session["EWorkItems"];
            RootObject returnWI = new RootObject();
            if (inp.ProjectName != null && inp.ProjectName != "Empty List" && inp.ProjectName != "0")
            {
                returnWI.value = new List<distribution_copy.Models.ExpandWI.Value>();
                foreach (var i in wI.value)
                {
                    if (i.fields.TeamProject == inp.ProjectName)
                        returnWI.value.Add(i);
                }
            }
            else
            {
                returnWI = wI;
            }

            RootObject returnWI2 = new RootObject();
            if (inp.WorkItemType != null && inp.WorkItemType != "Empty List" && inp.WorkItemType != "0")
            {
                returnWI2.value = new List<distribution_copy.Models.ExpandWI.Value>();
                foreach (var i in returnWI.value)
                {
                    if (i.fields.WorkItemType == inp.WorkItemType)
                        returnWI2.value.Add(i);
                }
            }
            else
                returnWI2 = returnWI;

            if (loc > 0)
                return returnWI2;
            else
            {
                string output = JsonConvert.SerializeObject(returnWI2.value);
                //return Json(FilteredWI.value, JsonRequestBehavior.AllowGet);
                return output;
            }

        }
        public JsonResult AllList(InputModel inp)
        {
            RootObject Witems = new RootObject();
            if (Session["EWorkItems"] == null)
                WITypes(inp);


            List<string> WorkItemList = new List<string>();
            List<string> AssignedToList = new List<string>();
            List<string> SprintList = new List<string>();
            List<string> StateList = new List<string>();
            foreach (var i in ((RootObject)Session["EWorkItems"]).value.Where(x => x.fields.TeamProject == inp.ProjectName))
            {
                if (i.fields.WorkItemType != null)
                {
                    if (!WorkItemList.Contains(i.fields.WorkItemType))
                        WorkItemList.Add(i.fields.WorkItemType);
                }
            }
            List<List<string>> all = new List<List<string>>();
            all.Add(AssignedToList);
            all.Add(SprintList);
            all.Add(StateList);
            all.Add(WorkItemList);
            return Json(all, JsonRequestBehavior.AllowGet);
        }
        public object Relation(int id, bool Export = false)
        {

            distribution_copy.Models.ExpandWI.Value v = new Models.ExpandWI.Value();
            v = ((RootObject)Session["EWorkItems"]).value.Find(x=>x.id==id);
            Dictionary<string, List<distribution_copy.Models.ExpandWI.Value>> rel = new Dictionary<string, List<distribution_copy.Models.ExpandWI.Value>>();

            var ForwardList = new List<Models.ExpandWI.Value>();
            var RevereseList = new List<Models.ExpandWI.Value>();
            if (v.relations != null)
            {
                foreach (var j in v.relations)
                {
                    if (j.rel == "System.LinkTypes.Hierarchy-Forward")
                    {
                        var valueArray = j.url.Split('/');
                        int Fid;
                        if (!(valueArray.Count() > 8))
                        {
                            continue;
                        }
                        if (int.TryParse(valueArray[8], out Fid))
                        {
                            var item = ((RootObject)Session["EWorkItems"]).value.Find(x => x.id == Fid);
                            ForwardList.Add(item);
                        }


                    }
                    else
                    {
                        var valueArray = j.url.Split('/');
                        int Fid;
                        if (!(valueArray.Count() > 8))
                        {
                            continue;
                        }
                        if (int.TryParse(valueArray[8], out Fid))
                        {
                            var item = ((RootObject)Session["EWorkItems"]).value.Find(x => x.id == Fid);
                            RevereseList.Add(item);
                        }
                    }
                }
                rel.Add("Forward", ForwardList);
                rel.Add("Reverse", RevereseList);
            }
            else
            {
                rel.Add("Forward", ForwardList);
                rel.Add("Reverse", RevereseList);
            }
            if (Export)
            {
                return rel;
            }
            return Json(rel, JsonRequestBehavior.AllowGet); 
        }

        public ExcelPackage TraceExport(TraceInputModel inp,bool flush=true)
        {
            InputModel inputModel = new InputModel();
            inputModel.OrganizationName = inp.OrgName;
            inputModel.ProjectName = inp.ProjectName;
            inputModel.WorkItemType = inp.WIType;
            var List = (RootObject)Filter(inputModel, 1);
            workSheet =  excel.Workbook.Worksheets.Add("WorkItems");
            workSheet.TabColor = System.Drawing.Color.White;
            workSheet.DefaultRowHeight = 12;
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "ID";
            workSheet.Cells[1, 2].Value = "Work Item Type";
           int j= AddColumns(List);
            workSheet.Cells[1,j++].Value = "Team Project";
            workSheet.Cells[1,j++].Value = "State";
            workSheet.Cells[1,j++].Value = "Area Path";
            workSheet.Cells[1,j++].Value = "Iteration";
            //workSheet.Cells[1,++j].Value = "Url";
            //workSheet.Cells[1,++j].Value = "PlannedHours";
            //workSheet.Cells[1,++j].Value = "ActualHours";
            //workSheet.Cells[1,++j].Value = "Sprint";
            //workSheet.Cells[1,++j].Value = "OriginalEstimate";
            //workSheet.Cells[1,++j].Value = "CompletedWork";
            //workSheet.Cells[1,++j].Value = "RemainingWork";
            //workSheet.Cells[1,++j].Value = "CreatedDate";
            //workSheet.Cells[1,++j].Value = "Description";
            //workSheet.Cells[1,++j].Value = "CreatedBy";
            //workSheet.Cells[1,++j].Value = "AssignedTo";
            //workSheet.Cells[1,++j].Value = "ChangedBy";
            recordIndex = 2;
            foreach (var i in List.value)
            {
                FindRelations(i,3);
            }
            for (var i = 1; i <= columnNo; i++)
                workSheet.Column(i).AutoFit();
            string excelName = inp.OrgName + "-" + (inp.ProjectName != null ? inp.ProjectName : "") + "-" + (inp.WIType != null ? inp.WIType : "") + DateTime.Now.ToString();

            if (flush)
            {
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    return excel;
                }
            }
            else {
                return excel;
            }
        }
        public void FindRelations(Models.ExpandWI.Value WI,int tInd)
        {
            int max = columnNo;
            int col = 0;
            workSheet.Cells[recordIndex, ++col].Value = WI.id;
            workSheet.Cells[recordIndex, ++col].Value = WI.fields.WorkItemType;
            workSheet.Cells[recordIndex,tInd].Value = WI.fields.Title;
            workSheet.Cells[recordIndex, columnNo++].Value = WI.fields.TeamProject;
            workSheet.Cells[recordIndex, columnNo++].Value = WI.fields.State;
                                     
            workSheet.Cells[recordIndex, columnNo++].Value = WI.fields.AreaPath;
            workSheet.Cells[recordIndex, columnNo++].Value = WI.fields.IterationPath;

            recordIndex++;
            columnNo = max;
            var Relations=  (Dictionary<string, List<distribution_copy.Models.ExpandWI.Value>>)Relation(WI.id,true);               
                    foreach(var j in Relations["Forward"]) {
                int n = tInd + 1;
                        FindRelations(j,n);
                    }           
           
        }
        public int AddColumns(RootObject root)
        {
            List<int> counts = new List<int>();

            foreach(var wi in root.value)
            {
              var count=  title(wi);
                i = 0;
                counts.Add(count);
            }
          int maxtitles=counts.Max();
            if (maxtitles == 1)
            {
                workSheet.Cells[1, 3].Value = "Title";
                this.columnNo = maxtitles + 3;
                return 4;
            }
            for(int k = 0; k <= maxtitles; k++)
            {
                workSheet.Cells[1, k+3].Value = "Title "+(k+1);
            }
            this.columnNo = maxtitles + 4;
            return maxtitles+4;
        }
        int v = 0;
        public int title(Models.ExpandWI.Value WI,int w=1)
        {
            var Relations = (Dictionary<string, List<distribution_copy.Models.ExpandWI.Value>>)Relation(WI.id, true);
           
            foreach (var j in Relations["Forward"])
            {
                title(j,w+1);
                if (i < w)
                {
                    i = w;
                }
            }
            return i;
        }
    }


}