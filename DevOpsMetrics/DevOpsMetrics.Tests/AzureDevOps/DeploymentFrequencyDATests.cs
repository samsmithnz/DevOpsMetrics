﻿using DevOpsMetrics.Service.DataAccess;
using DevOpsMetrics.Service.Models;
using DevOpsMetrics.Service.Models.AzureDevOps;
using DevOpsMetrics.Service.Models.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOpsMetrics.Tests.AzureDevOps
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestCategory("IntegrationTest")]
    [TestClass]
    public class DeploymentFrequencyDATests
    {
        public IConfigurationRoot Configuration;

        [TestInitialize]
        public void TestStartUp()
        {
            IConfigurationBuilder config = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json");
            Configuration = config.Build();
        }

        //[TestMethod]
        //public async Task AzDeploymentsDAIntegrationTest()
        //{
        //    //Arrange
        //    string patToken = Configuration["AppSettings:PatToken"];
        //    string organization = "samsmithnz";
        //    string project = "SamLearnsAzure";
        //    string branch = "refs/heads/master";
        //    string buildId = "3673"; //SamLearnsAzure.CI

        //    //Act
        //    BuildsDA da = new BuildsDA();
        //    List<AzureDevOpsBuild> list = await da.GetAzureDevOpsBuilds(patToken, organization, project, branch, buildId);

        //    //Assert
        //    Assert.IsTrue(list != null);
        //    Assert.IsTrue(list.Count > 0);
        //    Assert.IsTrue(list[0].status != null);
        //    Assert.IsTrue(list[0].buildDuration >= 0f);
        //    Assert.IsTrue(list[0].queueTime < list[1].queueTime);
        //}

        [TestMethod]
        public async Task AzDeploymentFrequencyDAIntegrationTest()
        {
            //Arrange
            bool getSampleData = true;
            string patToken = Configuration["AppSettings:PatToken"];
            string organization = "samsmithnz";
            string project = "SamLearnsAzure";
            string branch = "refs/heads/master";
            string buildName = "SamLearnsAzure.CI";
            string buildId = "3673"; //SamLearnsAzure.CI
            int numberOfDays = 7;

            //Act
            DeploymentFrequencyDA da = new DeploymentFrequencyDA();
            DeploymentFrequencyModel model = await da.GetAzureDevOpsDeploymentFrequency(getSampleData, patToken, organization, project, branch, buildName, buildId, numberOfDays);

            //Assert
            Assert.IsTrue(model.DeploymentsPerDayMetric > 0f);
            Assert.AreEqual(false, string.IsNullOrEmpty(model.DeploymentsPerDayMetricDescription));
            Assert.AreNotEqual("Unknown", model.DeploymentsPerDayMetricDescription);
        }

    }
}
