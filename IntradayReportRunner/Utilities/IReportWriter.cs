namespace IntradayReportRunner.Utilities
{
    public interface IReportWriter
    {
        void Write(string reportPathFullPath, string data);
    }
}
