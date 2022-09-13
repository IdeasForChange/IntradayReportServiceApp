using System.IO;

namespace IntradayReportRunner.Utilities
{
    internal class ReportWriter : IReportWriter
    {
        public void WriteCsv(string reportPathFullPath, string data)
        {
            FileInfo fileInfo = new FileInfo(reportPathFullPath);

            // Check if the directory exists. If not .. create the directory
            if(!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            // Write the result to the CSV file
            File.WriteAllText(reportPathFullPath, data);
        }
    }
}
