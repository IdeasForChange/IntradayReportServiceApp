using System;
using System.Threading.Tasks;

namespace IntradayReportRunner.Workflows
{
    public interface IIntradayReportRunnerWorkflow
    {
        Task ExecuteAsync(DateTime date);
    }
}
