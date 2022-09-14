using Services;

namespace IntradayReportService.Workflows.Utilities
{
    public interface IReportFormatter
    {
        string FormatTwoColumnIntradayTrade(PowerPeriod[] powerPeriods);
    }
}
