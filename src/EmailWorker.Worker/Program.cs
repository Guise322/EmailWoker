using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailWorker.Infrastructure;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using EmailWorker.ApplicationCore;

namespace EmailWorker.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string folderName = "logs";
            string fileName = "EmailWorkerLog-.txt";

            string logPath = Path.Combine(folderName, fileName);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    logPath, retainedFileCountLimit: 2,
                    rollingInterval: RollingInterval.Day,
                    retainedFileTimeLimit: TimeSpan.FromDays(1))
                .CreateLogger();
            try 
            {
                Log.Information("Starting up the EmailWorker service.");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The service does not started.");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()

                    .AddHttpClient()
                    
                    .AddApplicationCoreServices()

                    .AddInfrastructureServices();
                });
        }
    }
}