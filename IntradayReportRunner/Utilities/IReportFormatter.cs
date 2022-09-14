using Services;

namespace IntradayReportRunner.Utilities
{
    public interface IReportFormatter
    {
        string FormatTwoColumnIntradayTrade(PowerTrade powerTrade);
    }
}
