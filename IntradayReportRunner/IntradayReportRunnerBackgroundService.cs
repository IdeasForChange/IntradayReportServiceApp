using IntradayReportRunner.Workflows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IntradayReportRunner
{
    internal class IntradayReportRunnerBackgroundService : BackgroundService
    {
        private IIntradayReportRunnerWorkflow Workflow { get;  }
        public IConfiguration Configuration { get; }
        private ILogger<IntradayReportRunnerBackgroundService> Logger { get; }

        public double ExtractInterval => Configuration.GetValue<double>("IntradayReport:ExtractInterval");

        public IntradayReportRunnerBackgroundService (
            IIntradayReportRunnerWorkflow workflow,
            IConfiguration configuration,
            ILogger<IntradayReportRunnerBackgroundService> logger
            )
        {
            Workflow = workflow;
            Configuration = configuration;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation($"Running Intraday Report Runner Workflow @ {DateTime.Now}");
                    
                using var timer = new PeriodicTimer(TimeSpan.FromSeconds(ExtractInterval));
                while (true)
                {
                    await Workflow.ExecuteAsync(DateTime.Now);
                    await timer.WaitForNextTickAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Intraday Report Runner Workflow: ERROR - {ex.Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
