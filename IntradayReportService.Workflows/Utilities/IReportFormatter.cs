using Services;

namespace IntradayReportService.Workflows.Utilities
{
    public interface IReportFormatter
    {
        string FormatTwoColumnIntradayTrade(PowerTrade powerTrade);
    }
}
