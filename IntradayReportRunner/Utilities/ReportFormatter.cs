using Microsoft.Extensions.Configuration;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayReportRunner.Utilities
{
    public class ReportFormatter : IReportFormatter
    {
        public IConfiguration Configuration { get; }

        public string ResultTimeFormat => Configuration.GetValue<string>("IntradayReport:ResultTimeFormat");

        public ReportFormatter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string FormatTwoColumnIntradayTrade(PowerTrade powerTrade)
        {
            if (powerTrade is null)
                throw new ArgumentNullException($"Provided intraday trade object 'powerTrade' is null.");

            StringBuilder results = new StringBuilder("\"Local Time\",\"Volume\"\n");

            foreach (var item in powerTrade.Periods)
            {
                TimeSpan time = TimeSpan.FromHours(23 + (item.Period - 1));

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
