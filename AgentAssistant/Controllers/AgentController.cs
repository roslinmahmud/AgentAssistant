﻿using AgentAssistant.HubConfig;
using Entities.DTO;
using Entities.Models;
using Entities.Repository;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AgentAssistant.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentRepository agentRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<DashboardHub> hubContext;
        private readonly HttpClient httpClient;
        private readonly TimerManager timerManager;

        public AgentController(IAgentRepository agentRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<DashboardHub> hubContext,
            TimerManager timerManager)
        {
            this.agentRepository = agentRepository;
            this.userManager = userManager;
            this.hubContext = hubContext;
            this.timerManager = timerManager;
            HttpClientHandler httpClientHandler = new() { CookieContainer = new CookieContainer()};
            HttpClient httpClient = new(httpClientHandler)
            {
                BaseAddress = new Uri("https://agent.islamibankbd.com/"),
                Timeout = new TimeSpan(0, 0, 30)
            };
            this.httpClient = httpClient;

        }

        [HttpGet]
        public async Task<IActionResult> GetAgents()
        {
            var agents = await agentRepository.GetAllAgents();
            return Ok(agents);
        }

        [HttpGet("{id}")]
        public string GetAgent(string id)
        {
            return "value";
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateAgent([FromBody] Agent agent, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if(user == null)
                return BadRequest("Object is null");


            agentRepository.CreateAgent(agent);
            await agentRepository.SaveChangesAsync();

            user.AgentId = agent.Id;
            await agentRepository.SaveChangesAsync();

            return Created("api/Agent/", agent);
        }

        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            timerManager.Action = async () => await hubContext.Clients.All.SendAsync("transferSummaryData", await GetDataFromServer());
            timerManager.Timer.Change(0, 60000);

            return Ok(new { Message = "Request Completed" }); ;
        }

        private async Task<IActionResult> GetDataFromServer()
        {
            var summary = new SummaryResponseDto();
            try
            {
                await Login();

                summary.CurrentDayIncome = await GetCurrentDayIncome();
                summary.CurrentMonthIncome = await GetCurrentMonthIncome();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(summary);

        }

        private async Task Login()
        {
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", "nasrinnaharhena@gmail.com"),
                new KeyValuePair<string, string>("password", "Unlock1971")
            });

            var response = await httpClient.PostAsync("login01.do", form);
            response.EnsureSuccessStatusCode();
        }

        

        private async Task<string> GetCurrentDayIncome()
        {
            var response = await httpClient.GetAsync("reports/commission02.do");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var html = new HtmlDocument();
            html.LoadHtml(content);
            var h3 = html.DocumentNode.SelectNodes("//h3");

            if (h3[0].InnerHtml == "No Transaction!")
                return "0.00";

            return Math.Round(float.Parse(h3[1].InnerHtml.Split(" ")[2]), 2).ToString();
        }

        private async Task<string> GetCurrentMonthIncome()
        {
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("fromDate", DateTime.Now.AddDays(1-DateTime.Now.Day).Date.ToString("yyyy-MM-dd")),
                new KeyValuePair<string, string>("toDate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
            });

            var response = await httpClient.PostAsync("reports/commission02.do", form);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var html = new HtmlDocument();
            html.LoadHtml(content);
            var h3 = html.DocumentNode.SelectNodes("//h3");

            return Math.Round(float.Parse(h3[1].InnerHtml.Split(" ")[2]), 2).ToString();
        }


    }
}
