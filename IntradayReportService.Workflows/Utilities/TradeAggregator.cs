using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntradayReportService.Workflows.Utilities
{
    public class TradeAggregator : ITradeAggregator
    {
        public PowerTrade? AggregateIntradayTrade(IEnumerable<PowerTrade> powerTrades)
        {
            if (powerTrades is null)
                throw new ArgumentNullException($"Provided IEnumerable object 'powerTrades' is null.");

            var firstData = powerTrades.First();

            // Create a resultant PowerTrade which will be returned back after aggregation result
            PowerTrade results = PowerTrade.Create(firstData.Date, firstData.Periods.Length);

            // Just a simple check and making sure that the aggreagtion has to be done for same date  
            var allTrades = powerTrades.Where(p => p.Date == results.Date).SelectMany(p => p.Periods).ToList();

            // Lets aggregated the data as SelectMany() has already flattened our result set, 
            // So we just need to Group() the data by Period
            var aggregatedPowerPeriods = allTrades
                                            .GroupBy(p => p.Period)
                                            .Select(p => new PowerPeriod
                                            {
                                                Period = p.Key,
                                                Volume = p.Sum(g => g.Volume)
                                            }).ToArray();

            // I know that there are 24 periods defined. But I am just dynamically taking the number for iteration
            var numberOfPeriods = results.Periods.Count();

            // As the Periods is a read only property, we need to set the
            // results.Periods value from the aggregated results that we have received
            for (int period = 0; period < numberOfPeriods; period++)
            {
                results.Periods[period].Volume = aggregatedPowerPeriods[period].Volume;
            }

            return results;
        }
    }
}
