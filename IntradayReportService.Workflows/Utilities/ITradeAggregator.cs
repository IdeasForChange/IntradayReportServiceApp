using Services;
using System.Collections.Generic;

namespace IntradayReportService.Workflows.Utilities
{
    public interface ITradeAggregator
    {
        PowerPeriod[] AggregateIntradayTrade(IEnumerable<PowerTrade> powerTrades);
    }
}
