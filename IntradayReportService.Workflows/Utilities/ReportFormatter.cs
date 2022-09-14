using Microsoft.Extensions.Configuration;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntradayReportService.Workflows.Utilities
{
    public class ReportFormatter : IReportFormatter
    {
        public IConfiguration Configuration { get; }

        public string ResultTimeFormat => Configuration.GetValue<string>("IntradayReport:ResultTimeFormat");

        public ReportFormatter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string FormatTwoColumnIntradayTrade(PowerPeriod[] powerPeriods)
        {
            if (powerPeriods is null)
                throw new ArgumentNullException($"Provided intraday period object 'powerPeriods' is null.");

            StringBuilder results = new StringBuilder("\"Local Time\",\"Volume\"\n");

            foreach (var period in powerPeriods)
            {
                TimeSpan time = TimeSpan.FromHours(23 + (period.Period - 1));

                var data = new List<string>();
                data.Add(time.ToString(ResultTimeFormat));
                data.Add(period.Volume.ToString());

                results.Append(string.Join(",", data));
                results.Append("\n");
            }
            return results.ToString();
        }
    }
}
