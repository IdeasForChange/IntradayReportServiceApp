using IntradayReportRunner.Utilities;
using IntradayReportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.IO;

namespace IntradayReportRunner.Workflows
{
    public class IntradayReportRunnerWorkflow : IIntradayReportRunnerWorkflow
    {
        public IPowerServiceProxy PowerServiceProxy { get; }
        public IConfiguration Configuration { get; }        
        public IReportWriter ReportWriter { get; }
        public ILogger<IntradayReportRunnerWorkflow> Logger { get; }

        public string ReportPath => Configuration.GetValue<string>("IntradayReport:Path");
        public string ReportNamePattern => Configuration.GetValue<string>("IntradayReport:ReportNamePattern");
        public string DataFormat => Configuration.GetValue<string>("IntradayReport:DateFormat");
        public string TimeFormat => Configuration.GetValue<string>("IntradayReport:TimeFormat");
        public string ResultTimeFormat => Configuration.GetValue<string>("IntradayReport:ResultTimeFormat"); 

        public string ReportName => ReportNamePattern.Replace(DataFormat.ToUpper(), DateTime.Now.ToString(DataFormat))
            .Replace(TimeFormat.ToUpper(), DateTime.Now.ToString(TimeFormat));

        public string FullReportPath => Path.Combine(ReportPath, ReportName);

        public IntradayReportRunnerWorkflow(
            ILogger<IntradayReportRunnerWorkflow> logger,
            IConfiguration configuration,
            IPowerServiceProxy powerServiceProxy,
            IReportWriter reportWriter
            )
        {
            PowerServiceProxy = powerServiceProxy;
            Configuration = configuration;
            ReportWriter = reportWriter;
            Logger = logger;
        }

        public async Task ExecuteAsync(DateTime date)
        {
            // Workflow has two tasks
            Logger.LogInformation("Workflow Execution: STARTED");

            // 1. Fetch Data Asynchronously
            var powerTrades = await PowerServiceProxy.GetTradesAsync(date);

            // 2. Write data to the CSV file
            if (powerTrades != null)
            {
                // Created aggregated results
                var aggregatedResults = AggreagatePowerTrades(powerTrades);

                // Create the formatted results in String format
                var formattedData= CreatedFormattedResults(aggregatedResults);

                // Write the results
                ReportWriter.WriteCsv(FullReportPath, formattedData);
            }

            Logger.LogInformation("Workflow Execution: COMPLETED");
        }

        private PowerTrade AggreagatePowerTrades(IEnumerable<PowerTrade> powerTrades)
        {
            PowerTrade results = null; 
            foreach(PowerTrade powerTrade in powerTrades)
            {
                results ??= powerTrade;

                if (results.Date == powerTrade.Date)
                {
                    foreach(var powerPeriod in powerTrade.Periods)
                    {
                        foreach(var period in results.Periods)
                        {
                            if (period.Period == powerPeriod.Period)
                            {
                                period.Volume += powerPeriod.Volume;
                            }
                        }
                    }
                }
            }
            return results;
        }

        private string CreatedFormattedResults(PowerTrade powerTrade)
        {
            StringBuilder results = new StringBuilder("\"Local Time\",\"Volume\"\n");

            foreach(var item in powerTrade.Periods)
            {
                TimeSpan time = TimeSpan.FromHours(23 + (item.Period-1));

                var data = new List<string>();
                data.Add(time.ToString(ResultTimeFormat));
                data.Add(item.Volume.ToString());

                results.Append(string.Join(",", data));
                results.Append("\n");
            }

            return results.ToString();
        }
    }
}
