using Microsoft.Extensions.Logging;
using Services;

namespace IntradayReportService
{
    public class PowerServiceProxy : IPowerServiceProxy
    {
        private readonly IPowerService _powerService;
        private readonly ILogger<PowerServiceProxy> _logger;

        public PowerServiceProxy(
            IPowerService powerService, 
            ILogger<PowerServiceProxy> logger)
        {
            _powerService = powerService;
            _logger = logger;
        }

        public IEnumerable<PowerTrade> GetTrades(DateTime date)
        {
            _logger.LogInformation("Calling GetTrades(): STARTED");
            var results = _powerService.GetTrades(date);
            _logger.LogInformation("Calling GetTrades(): COMPLETED");

            return results;
        }

        public async Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date)
        {
            _logger.LogInformation("Calling GetTradesAsync(): STARTED");
            var results = await _powerService.GetTradesAsync(date);
            _logger.LogInformation("Calling GetTradesAsync(): COMPLETED");

            return results;
        }
    }
}
