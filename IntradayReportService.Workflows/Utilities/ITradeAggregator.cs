using Services;
using System.Collections.Generic;

namespace IntradayReportService.Workflows.Utilities
{
    public interface ITradeAggregator
    {
        PowerTrade? AggregateIntradayTrade(IEnumerable<PowerTrade> powerTrades);
    }
}
