﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevOpsMetrics.Web.Models;
using Microsoft.Extensions.Configuration;
using DevOpsMetrics.Service.Models;

namespace DevOpsMetrics.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //Azure DevOps
            string patToken = _configuration["AppSettings:PatToken"];
            string organization = "samsmithnz";
            string project = "SamLearnsAzure";
            string azBranch = "refs/heads/master";
            string buildId = "3673"; //SamLearnsAzure.CI

            //GitHub
            string owner = "samsmithnz";
            string repo = "samsfeatureflags";
            string ghbranch = "master";
            string workflowId = "108084";

            ServiceApiClient service = new ServiceApiClient(_configuration);
            List<AzureDevOpsBuild> azList = await service.GetAZDeployments(patToken, organization, project, azBranch, buildId);
            List<GitHubActionsRun> ghList = await service.GetGHDeployments(owner, repo, ghbranch, workflowId);

            IndexDeploymentModel indexModel = new IndexDeploymentModel();

            if (azList.Count < 10)
            {
                indexModel.AZList = azList;
            }
            else
            {
                indexModel.AZList = new List<AzureDevOpsBuild>();
                //Only show the last ten builds
                for (int i = azList.Count - 10; i < azList.Count; i++)
                {
                    indexModel.AZList.Add(azList[i]);
                }
            }
            indexModel.GHList = ghList;

            return View(indexModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
