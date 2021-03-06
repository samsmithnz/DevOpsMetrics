﻿using DevOpsMetrics.Core.Models.Common;
using Microsoft.Extensions.Configuration;

namespace DevOpsMetrics.Service
{
    public class Common
    {
        public static TableStorageConfiguration GenerateTableStorageConfiguration(IConfiguration Configuration)
        {
            TableStorageConfiguration tableStorageConfig = new()
            {
                StorageAccountConnectionString = Configuration["AppSettings:AzureStorageAccountConfigurationString"],
                TableAzureDevOpsBuilds = Configuration["AppSettings:AzureStorageAccountContainerAzureDevOpsBuilds"],
                TableAzureDevOpsPRs = Configuration["AppSettings:AzureStorageAccountContainerAzureDevOpsPRs"],
                TableAzureDevOpsPRCommits = Configuration["AppSettings:AzureStorageAccountContainerAzureDevOpsPRCommits"],
                TableAzureDevOpsSettings = Configuration["AppSettings:AzureStorageAccountContainerAzureDevOpsSettings"],
                TableGitHubRuns = Configuration["AppSettings:AzureStorageAccountContainerGitHubRuns"],
                TableGitHubPRs = Configuration["AppSettings:AzureStorageAccountContainerGitHubPRs"],
                TableGitHubPRCommits = Configuration["AppSettings:AzureStorageAccountContainerGitHubPRCommits"],
                TableGitHubSettings = Configuration["AppSettings:AzureStorageAccountContainerGitHubSettings"],
                TableMTTR = Configuration["AppSettings:AzureStorageAccountContainerMTTR"],
                TableChangeFailureRate = Configuration["AppSettings:AzureStorageAccountContainerChangeFailureRate"],
                TableLog = Configuration["AppSettings:AzureStorageAccountContainerTableLog"]
            };
            return tableStorageConfig;
        }
    }
}
