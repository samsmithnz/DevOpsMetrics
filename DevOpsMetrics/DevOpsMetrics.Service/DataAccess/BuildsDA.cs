﻿using DevOpsMetrics.Core;
using DevOpsMetrics.Service.Models.AzureDevOps;
using DevOpsMetrics.Service.Models.Common;
using DevOpsMetrics.Service.Models.GitHub;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOpsMetrics.Service.DataAccess
{
    public class BuildsDA
    {
        public async Task<Newtonsoft.Json.Linq.JArray> GetAzureDevOpsBuildsJArray(string patToken, string organization, string project, string branch, string buildId)
        {
            Newtonsoft.Json.Linq.JArray list = null;
            string url = $"https://dev.azure.com/{organization}/{project}/_apis/build/builds?api-version=5.1&queryOrder=BuildQueryOrder,finishTimeDescending";
            string response = await MessageUtility.SendAzureDevOpsMessage(url, patToken);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                list = jsonObj.value;
            }

            return list;
        }

        public async Task<List<AzureDevOpsBuild>> GetAzureDevOpsBuilds(string patToken, string organization, string project, string branch, string buildId)
        {
            List<AzureDevOpsBuild> builds = new List<AzureDevOpsBuild>();
            Newtonsoft.Json.Linq.JArray list = await GetAzureDevOpsBuildsJArray(patToken, organization, project, branch, buildId);
            if (list != null)
            {
                builds = JsonConvert.DeserializeObject<List<AzureDevOpsBuild>>(list.ToString());

                //We need to do some post processing and loop over the list a couple times to find the max build duration, construct a usable url, and calculate a build duration percentage
                float maxBuildDuration = 0f;
                foreach (AzureDevOpsBuild item in builds)
                {
                    //construct and add in the Url to each build
                    item.url = $"https://dev.azure.com/{organization}/{project}/_build/results?buildId={item.id}&view=results";
                    if (item.buildDuration > maxBuildDuration)
                    {
                        maxBuildDuration = item.buildDuration;
                    }
                }
                foreach (AzureDevOpsBuild item in builds)
                {
                    float interiumResult = ((item.buildDuration / maxBuildDuration) * 100f);
                    item.buildDurationPercent = Utility.ScaleNumberToRange(interiumResult, 0, 100, 20, 100);
                }

                //sort the list
                builds = builds.OrderBy(o => o.queueTime).ToList();
            }


            return builds;
        }

        public async Task<List<GitHubActionsRun>> GetGitHubActionRuns(bool getSampleData, string clientId, string clientSecret, string owner, string repo, string branch, string workflowId)
        {
            List<GitHubActionsRun> runs = new List<GitHubActionsRun>();
            string url = $"https://api.github.com/repos/{owner}/{repo}/actions/workflows/{workflowId}/runs?per_page=100";
            string response = await MessageUtility.SendGitHubMessage(url, clientId, clientSecret);
            if (string.IsNullOrEmpty(response) == false)
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(response);
                Newtonsoft.Json.Linq.JArray value = jsonObj.workflow_runs;
                runs = JsonConvert.DeserializeObject<List<GitHubActionsRun>>(value.ToString());

                //We need to do some post processing and loop over the list a couple times to find the max build duration, and calculate a build duration percentage
                float maxBuildDuration = 0f;
                foreach (GitHubActionsRun item in runs)
                {
                    if (item.buildDuration > maxBuildDuration)
                    {
                        maxBuildDuration = item.buildDuration;
                    }
                }
                foreach (GitHubActionsRun item in runs)
                {
                    float interiumResult = ((item.buildDuration / maxBuildDuration) * 100f);
                    item.buildDurationPercent = Utility.ScaleNumberToRange(interiumResult, 0, 100, 20, 100);
                }

                //sort the list
                runs = runs.OrderBy(o => o.created_at).ToList();
            }

            return runs;
        }

    }
}
