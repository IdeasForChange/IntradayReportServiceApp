using IntradayReportRunner.Utilities;
using IntradayReportRunner.Workflows;
using IntradayReportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services;
using System;
using System.Threading.Tasks;

namespace IntradayReportRunner
{
    public class Program
    {
        private static readonly ILoggerFactory _loggerFactory;
        private static ILogger<Program> Logger { get; set; }

        private static string AppEnvironment { get; set; }

        static Program()
        {
            // Create Logger and Logger Factory
            _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            Logger = _loggerFactory.CreateLogger<Program>();

            // Identify if this application is running in a specific environment
            AppEnvironment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "dev";
            Logger.LogInformation($"Application stating for environment: {AppEnvironment}");
        }

        public static async Task Main(string[] args)
        {
            // Make sure you handle any exception in the command line job 
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Set the environment base directory to the location from where this application is running
            Environment.CurrentDirectory = AppContext.BaseDirectory;

            // Initialise configuration builder
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = BuildHost(args);

            // Instantiate Runner Workflow to execute the seuential workflow.
            try
            {
                var workflow = host.Services.GetRequiredService<IIntradayReportRunnerWorkflow>();
                await workflow.ExecuteAsync(DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.LogError($"INNER EXCEPTION  :   {ex.Message}");
                Logger.LogError($"STACK TRACE      :   {ex.StackTrace}");
                Environment.Exit(1);
            }

            // Success .. exit 
            Logger.LogInformation("Program successfully completed!");
            Environment.Exit(0);
        }

        static IHost BuildHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Inject dependencies
                    services.AddScoped<IPowerService, PowerService>();
                    services.AddScoped<IPowerServiceProxy, PowerServiceProxy>();
                    services.AddScoped<IReportFormatter, ReportFormatter>();
                    services.AddScoped<IReportWriter, ReportWriter>();
                    services.AddScoped<ITradeAggregator, TradeAggregator>();

                    services.AddScoped<IIntradayReportRunnerWorkflow, IntradayReportRunnerWorkflow>();
                })
                .Build();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{AppEnvironment}.json", optional: true, reloadOnChange: true)
                .Build(); 
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;

            if (exception != null)
            {
                if (exception.InnerException != null)
                {
                    Logger.LogError($"INNER EXCEPTION                    :   {exception.InnerException.Message}");
                    Logger.LogError($"STACK TRACE                        :   {exception.InnerException.StackTrace}");
                }

                Logger.LogError($"APPLICATION EXCEPTION                  :   {exception.Message}");
                Logger.LogError($"STACK TRACE                            :   {exception.StackTrace}");
            }
            Environment.Exit(-1);
        }
    }
}
