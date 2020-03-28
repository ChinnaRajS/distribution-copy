using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using distribution_copy.Models.AccessDetails;
using distribution_copy.Models.AccountsResponse;
using distribution_copy.Models.ProfileDetails;
using distribution_copy.Services;

namespace distribution_copy.Controllers
{
    public class WIReportController : Controller
    {
        readonly AccountService Account = new AccountService();
        // GET: WIReport
        public ActionResult Index()
        {
            if (Session["visited"]==null)
                return RedirectToAction("../Account/Verify");

            if (Session["PAT"] == null) { 
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
            catch (Exception){}
            }
            return View();
        }                   
    }
}
