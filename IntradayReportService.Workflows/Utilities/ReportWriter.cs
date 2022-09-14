using System;
using System.IO;

namespace IntradayReportService.Workflows.Utilities
{
    public class ReportWriter : IReportWriter
    {
        public void Write(string reportPathFullPath, string data)
        {
            FileInfo fileInfo = new FileInfo(reportPathFullPath);

            if (string.IsNullOrEmpty(fileInfo.DirectoryName))
                throw new ArgumentNullException($"Directory Name is NULL or empty: {reportPathFullPath}");

            // Check if the directory exists. If not .. create the directory
            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            // Write the result to the CSV file
            File.WriteAllText(reportPathFullPath, data);
        }
    }
}
