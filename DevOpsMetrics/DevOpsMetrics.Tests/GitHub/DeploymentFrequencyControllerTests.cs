﻿using DevOpsMetrics.Service.Controllers;
using DevOpsMetrics.Service.Models;
using DevOpsMetrics.Service.Models.Common;
using DevOpsMetrics.Service.Models.GitHub;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevOpsMetrics.Tests.GitHub
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestCategory("IntegrationTest")]
    [TestClass]
    public class DeploymentFrequencyControllerTests
    {

        private TestServer _server;
        public HttpClient Client;
        public IConfigurationRoot Configuration;

        [TestInitialize]
        public void TestStartUp()
        {
            IConfigurationBuilder config = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json");
            Configuration = config.Build();

            //Setup the test server
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<DevOpsMetrics.Service.Startup>());
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri(Configuration["AppSettings:WebServiceURL"]);
        }

        //[TestCategory("ControllerTest")]
        //[TestMethod]
        //public async Task GHDeploymentsControllerIntegrationTest()
        //{
        //    //Arrange
        //    bool getSampleData = true;
        //    string clientId = "";
        //    string clientSecret = "";
        //    string owner = "samsmithnz";
        //    string repo = "samsfeatureflags";
        //    string branch = "master";
        //    string workflowId = "108084";
        //    DeploymentFrequencyController controller = new DeploymentFrequencyController();

        //    //Act
        //    List<GitHubActionsRun> list = await controller.GetGitHubDeployments(getSampleData, clientId, clientSecret, owner, repo, branch, workflowId);

        //    //Assert
        //    Assert.IsTrue(list != null);
        //    Assert.IsTrue(list.Count > 0);
        //    Assert.IsTrue(list[0].status != null);
        //}

        [TestCategory("ControllerTest")]
        [TestMethod]
        public async Task GHDeploymentFrequencyControllerIntegrationTest()
        {
            //Arrange
            bool getSampleData = true;
            string clientId = "";
            string clientSecret = "";
            string owner = "samsmithnz";
            string repo = "samsfeatureflags";
            string branch = "master";
            string workflowName = "samsfeatureflags CI/CD";
            string workflowId = "108084";
            int numberOfDays = 7;
            DeploymentFrequencyController controller = new DeploymentFrequencyController();

            //Act
            DeploymentFrequencyModel model = await controller.GetGitHubDeploymentFrequency(getSampleData, clientId, clientSecret, owner, repo, branch, workflowName, workflowId, numberOfDays);

            //Assert
            Assert.IsTrue(model.DeploymentsPerDayMetric > 0f);
            Assert.IsTrue(model.BuildList.Count > 0);
            Assert.AreEqual(false, string.IsNullOrEmpty(model.DeploymentsPerDayMetricDescription));
            Assert.AreNotEqual("Unknown", model.DeploymentsPerDayMetricDescription);
        }

        //[TestCategory("APITest")]
        //[TestMethod]
        //public async Task GHDeploymentsControllerAPIIntegrationTest()
        //{
        //    //Arrange
        //    bool getSampleData = false;
        //    string clientId = "";
        //    string clientSecret = "";
        //    string owner = "samsmithnz";
        //    string repo = "samsfeatureflags";
        //    string branch = "master";
        //    string workflowName = "samsfeatureflags CI/CD";
        //    string workflowId = "108084";

        //    //Act
        //    string url = $"/api/DeploymentFrequency/GetGitHubDeployments?getSampleData={getSampleData}&clientId={clientId}&clientSecret={clientSecret}&owner={owner}&repo={repo}&GHbranch={branch}&workflowId={workflowId}";
        //    TestResponse<List<GitHubActionsRun>> httpResponse = new TestResponse<List<GitHubActionsRun>>();
        //    List<GitHubActionsRun> list = await httpResponse.GetResponse(Client, url);

        //    //Assert
        //    Assert.IsTrue(list != null);
        //    Assert.IsTrue(list.Count > 0);
        //    Assert.IsTrue(list[0].status != null);
        //}

    }
}
