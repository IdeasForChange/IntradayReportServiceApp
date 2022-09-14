using IntradayReportRunner.Utilities;
using IntradayReportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.IO;

namespace IntradayReportRunner.Workflows
{
    public class IntradayReportRunnerWorkflow : IIntradayReportRunnerWorkflow
    {
        public IPowerServiceProxy PowerServiceProxy { get; }
        public ITradeAggregator TradeAggregator { get; }
        public IReportFormatter ReportFormatter { get; }
        public IConfiguration Configuration { get; }        
        public IReportWriter ReportWriter { get; }
        public ILogger<IntradayReportRunnerWorkflow> Logger { get; }

        public string ReportPath => Configuration.GetValue<string>("IntradayReport:Path");
        public string ReportNamePattern => Configuration.GetValue<string>("IntradayReport:ReportNamePattern");
        public string DataFormat => Configuration.GetValue<string>("IntradayReport:DateFormat");
        public string TimeFormat => Configuration.GetValue<string>("IntradayReport:TimeFormat");

        public string ReportName => ReportNamePattern.Replace(DataFormat.ToUpper(), DateTime.Now.ToString(DataFormat))
            .Replace(TimeFormat.ToUpper(), DateTime.Now.ToString(TimeFormat));

        public string FullReportPath => Path.Combine(ReportPath, ReportName);

        public IntradayReportRunnerWorkflow(
            ILogger<IntradayReportRunnerWorkflow> logger,
            IConfiguration configuration,
            IPowerServiceProxy powerServiceProxy,
            ITradeAggregator tradeAggregator,
            IReportFormatter reportFormatter,
            IReportWriter reportWriter
            )
        {
            PowerServiceProxy = powerServiceProxy;
            TradeAggregator = tradeAggregator;
            ReportFormatter = reportFormatter;
            Configuration = configuration;
            ReportWriter = reportWriter;
            Logger = logger;
        }

        public async Task ExecuteAsync(DateTime date)
        {
            Logger.LogInformation("Workflow Execution: STARTED");

            // 1. Fetch Data Asynchronously
            var powerTrades = await PowerServiceProxy.GetTradesAsync(date);

            // 2. Write data to the CSV file
            if (powerTrades != null)
            {
                // 3. Created aggregated results
                var aggregatedResults = TradeAggregator.AggregateIntradayTrade(powerTrades);

                if(aggregatedResults != null)
                {
                    // 4. Create the formatted results in String format
                    var formattedData = ReportFormatter.FormatTwoColumnIntradayTrade(aggregatedResults);

                    // 5. Write the results
                    ReportWriter.Write(FullReportPath, formattedData);
                }
            }
            Logger.LogInformation("Workflow Execution: COMPLETED");
        }
    }
}
