namespace IntradayReportService.Workflows.Utilities
{
    public interface IReportWriter
    {
        void Write(string reportPathFullPath, string data);
    }
}
