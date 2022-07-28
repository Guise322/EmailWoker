using Microsoft.Extensions.Hosting;
using EmailWorker.Infrastructure;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using EmailWorker.Application;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Worker
{
    public class Program
    {
        private const string _folderName = "logs";
        private const string _fileName = "EmailWorkerLog-.txt";

        public static void Main(string[] args)
        {
            string logPath = Path.Combine(_folderName, _fileName);

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
                        .AddApplicationServices()
                        .AddInfrastructureServices();
                });
        }
    }
}