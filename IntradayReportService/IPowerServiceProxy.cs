using Services;

namespace IntradayReportService
{
    public interface IPowerServiceProxy
    {
        IEnumerable<PowerTrade> GetTrades(DateTime date);
        Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date);
    }
}
