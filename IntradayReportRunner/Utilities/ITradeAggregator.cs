using Services;
using System.Collections.Generic;

namespace IntradayReportRunner.Utilities
{
    public interface ITradeAggregator
    {
        PowerTrade? AggregateIntradayTrade(IEnumerable<PowerTrade> powerTrades);
    }
}
