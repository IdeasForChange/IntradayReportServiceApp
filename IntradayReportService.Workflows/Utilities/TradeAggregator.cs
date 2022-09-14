using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntradayReportService.Workflows.Utilities
{
    public class TradeAggregator : ITradeAggregator
    {
        public PowerPeriod[] AggregateIntradayTrade(IEnumerable<PowerTrade> powerTrades)
        {
            if (powerTrades is null)
                throw new ArgumentNullException($"Provided IEnumerable object 'powerTrades' is null.");

            // Just a simple check and making sure that the aggreagtion has to be done for same date  
            var allTrades = powerTrades
                                .Where(p => p.Date == powerTrades.First().Date)
                                .SelectMany(p => p.Periods)
                                .ToList();

            // Lets aggregated the data as SelectMany() has already flattened our result set, 
            // So we just need to Group() the data by Period
            var aggregatedPowerPeriods = allTrades
                                            .GroupBy(p => p.Period)
                                            .Select(p => new PowerPeriod
                                            {
                                                Period = p.Key,
                                                Volume = p.Sum(g => g.Volume)
                                            }).ToArray();

            return aggregatedPowerPeriods;
        }
    }
}
