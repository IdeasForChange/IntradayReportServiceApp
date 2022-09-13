namespace IntradayReportRunner.Utilities
{
    public interface IReportWriter
    {
        void WriteCsv(string reportPathFullPath, string data);
    }
}
