using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DevOpsMetrics.Service.Models.AzureDevOps;
using DevOpsMetrics.Service.Models.GitHub;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevOpsMetrics.NightlyProcessor.Function
{
    public static class NightlyProcessor
    {
        [FunctionName("UpdateStorageTables")]
        public static async Task Run([TimerTrigger("0 */2 * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            //Load settings
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                .AddEnvironmentVariables()
                .Build();

            //Get settings
            ServiceApiClient api = new ServiceApiClient(configuration);
            List<AzureDevOpsSettings> azSettings = await api.GetAzureDevOpsSettings();
            List<GitHubSettings> ghSettings = await api.GetGitHubSettings();

            //Loop through each setting to update the runs, pull requests and pull request commits
            foreach (AzureDevOpsSettings item in azSettings)
            {

            }
            foreach (GitHubSettings item in ghSettings)
            {

            }
        }

    }
}
