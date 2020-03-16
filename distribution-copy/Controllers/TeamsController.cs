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
using distribution_copy.Helper;

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

        public JsonResult IterationsList(string ORG, string project)
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

                    using (HttpResponseMessage response = client.GetAsync("https://dev.azure.com/" + ORG + "/_apis/projects/" + project + "/teams?api-version=5.1").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        responseBody = response.Content.ReadAsStringAsync().Result;
                        teams = JsonConvert.DeserializeObject<TeamDetails>(responseBody);
                    }
                }
                if (teams.value != null && teams.value.Count > 0)
                {
                    foreach (var team in teams.value)
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
            Session["iterationsList"] = iterationsList;
            return Json(iterationsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CapacityReport(string org, string project, string iterationPath)
        {
            List<IterationDetails> iterationsList = new List<IterationDetails>();
            List<string> teamnames = new List<string>();
            List<Iterations> iterations = new List<Iterations>();
            if (Session["iterationsList"] != null)
            {
                iterationsList = (List<IterationDetails>)Session["iterationsList"];
                foreach (IterationDetails item in iterationsList)
                {
                    if (item.value.Count > 0)
                    {
                        var iteration = item.value.Where(x => x.path == iterationPath).ToList();
                        var result = iteration.ToList();
                        foreach (var sprint in result)
                        {
                            sprint.team = item.teamname;
                            iterations.Add(sprint);
                        }
                    }
                }
            }
            TeamCapacity capacity = GetTeamCapacityDetails(org, project, iterations);

            return Json(capacity, JsonRequestBehavior.AllowGet);
        }

        public TeamCapacity GetTeamCapacityDetails(string org, string project, List<Iterations> iterations)
        {
            string responseBody = "";
            TeamCapacity capacity = new TeamCapacity();
            capacity.currentTeamCapacities = new List<CurrentTeamCapacity>();
            capacity.totalTeamCapacities = new List<TotalTeamCapacity>();
            capacity.capacitybyTeamMembers = new List<CapacitybyTeamMember>();
            capacity.leavesbyTeamMembers = new List<LeavesbyTeamMember>();
            List<CapacityDetails> capacityList = new List<CapacityDetails>();
            foreach (var item in iterations)
            {
                CapacityDetails capacitydetails = new CapacityDetails();
                string teamname = item.team;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", Session["PAT"] == null ? Request.QueryString["code"] : Session["PAT"].ToString()))));
                    string url = "https://dev.azure.com/" + org + "/" + project + "/" + teamname + "/_apis/work/teamsettings/iterations/" + item.id + "/capacities?api-version=5.1";
                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        response.EnsureSuccessStatusCode();
                        responseBody = response.Content.ReadAsStringAsync().Result;
                        capacitydetails = JsonConvert.DeserializeObject<CapacityDetails>(responseBody);
                        capacitydetails.teamName = item.team;
                        capacitydetails.IterationPath = item.path;
                        capacityList.Add(capacitydetails);
                    }
                }
            }
            foreach (var item in capacityList)
            {
                try
                {
                    var teamiteration = iterations.FirstOrDefault(x => x.path == item.IterationPath && x.team == item.teamName);
                    CurrentTeamCapacity currentTeamCapacity = new CurrentTeamCapacity();
                    TotalTeamCapacity totalTeamCapacity = new TotalTeamCapacity();
                    double currentCapacity = 0;
                    double TotalCapacity = 0;
                    if (teamiteration != null)
                    {
                        currentTeamCapacity.iterationPath = teamiteration.path;
                        currentTeamCapacity.teamName = teamiteration.team;
                        double currDays = Convert.ToDouble(DateTime.Now.ToString().GetBusinessDays(teamiteration.attributes.finishDate));
                        if(currDays>0 && currDays < 1)
                        {
                            currDays = 1;
                        }
                        currentTeamCapacity.currentWorkingDays= currDays < 0 ? "0" :Convert.ToInt32(currDays).ToString();
                        totalTeamCapacity.iterationPath = teamiteration.path;
                        totalTeamCapacity.teamName = teamiteration.team;
                        totalTeamCapacity.iterationStart = teamiteration.attributes.startDate;
                        totalTeamCapacity.iterationEnd = teamiteration.attributes.finishDate;
                        totalTeamCapacity.totalWorkingDays= teamiteration.attributes.startDate.GetBusinessDays(teamiteration.attributes.finishDate);

                        foreach (var member in item.value)
                        {
                            currentCapacity += (Convert.ToDouble(member.activities[0].capacityPerDay));
                        }
                        currentTeamCapacity.currentCapacity = (currentCapacity * Convert.ToDouble(currentTeamCapacity.currentWorkingDays)).ToString();
                        totalTeamCapacity.totalCapacity = (currentCapacity * Convert.ToDouble(totalTeamCapacity.totalWorkingDays)).ToString();
                        capacity.currentTeamCapacities.Add(currentTeamCapacity);
                        capacity.totalTeamCapacities.Add(totalTeamCapacity);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return capacity;
        }


    }
}