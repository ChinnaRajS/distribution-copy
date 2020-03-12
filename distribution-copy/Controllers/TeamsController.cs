using distribution_copy.Models.AccountsResponse;
using distribution_copy.Models.InputModel;
using distribution_copy.Models.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using distribution_copy.Models;

namespace distribution_copy.Controllers
{
    public class TeamsController : Controller
    {
        // GET: Teams
        public ActionResult Capacity()
        {
            InputModel input = new InputModel();           
            return View(input);
        }

        public JsonResult IterationsList(string ORG,string project)
        {
            TeamDetails teams = new TeamDetails();            
            List<IterationDetails> iterationsList = new List<IterationDetails>();
            string responseBody = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString()))));
                    
                    using (HttpResponseMessage response = client.GetAsync("https://dev.azure.com/" + ORG + "/_apis/projects/"+ project + "/teams?api-version=5.1").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        responseBody = response.Content.ReadAsStringAsync().Result;
                        teams = JsonConvert.DeserializeObject<TeamDetails>(responseBody);
                    }
                }                
                if(teams.value!=null && teams.value.Count > 0)
                {
                    foreach(var team in teams.value)
                    {
                        IterationDetails iterationDetails = new IterationDetails();
                        string teamname = team.name;
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(
                                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                                    System.Text.ASCIIEncoding.ASCII.GetBytes(
                                        string.Format("{0}:{1}", "", Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString()))));
                            string url = "https://dev.azure.com/" + ORG + "/" + project + "/" + teamname + "/_apis/work/teamsettings/iterations?api-version=5.1";
                            using (HttpResponseMessage response = client.GetAsync(url).Result)
                            {
                                response.EnsureSuccessStatusCode();
                                responseBody = response.Content.ReadAsStringAsync().Result;
                                iterationDetails = JsonConvert.DeserializeObject<IterationDetails>(responseBody);
                                iterationDetails.teamname = teamname;
                                iterationsList.Add(iterationDetails);
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                return Json("");
            }

            return Json(iterationsList, JsonRequestBehavior.AllowGet);
        }
    }
}