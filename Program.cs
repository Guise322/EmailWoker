using EmailWorker.ApplicationCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.DomainServices;
using EmailWorker.Infrastructure;
using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using System;
using System.IO;

namespace EmailWorker
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
                    .AddTransient<IEntryPointService, EntryPointService>()

                    .AddTransient<EmailCredentialsGetter>()

                    .AddScoped<IAnswerSender, AnswerSender>()
                    .AddScoped<IGetterOfUnseenMessages, GetterOfUnseenMessages>()
                    .AddScoped<IHandlerOfAsSeenMarkerMessages, HandlerOfAsSeenMarkerMessages>()
                    .AddScoped<IClientConnector, ClientConnector>()

                    .AddScoped<IHandlerOfPublicIPGetterMessages, HandlerOfPublicIPGetterMessages>()
                    .AddScoped<IMessageGetter, MessageGetter>()

                    .AddScoped<IAsSeenMarkerService, AsSeenMarkerService>()
                    .AddScoped<IPublicIPGetterService, PublicIPGetterService>()

                    .AddScoped<ImapClient>();
                });
        }
    }
}
