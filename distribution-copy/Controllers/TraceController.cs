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

namespace distribution_copy.Controllers
{
    public class TraceController : Controller
    {
        public string url = "";

        AccountService Account = new AccountService();

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
        public JsonResult Relation(int id)
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
                return Json(rel, JsonRequestBehavior.AllowGet);
            } 
            return Json(rel, JsonRequestBehavior.AllowGet); 
        }
    }
}