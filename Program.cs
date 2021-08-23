using EmailWorker.ApplicationCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxProcessorAggregate;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerAggregate;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterAggregate;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.DomainServices;
using EmailWorker.Infrastructure;
using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using System;

namespace EmailWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"/var/log/EmailWorkerLog.txt")
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()
                    .AddTransient<IEntryPointService, EntryPointService>()

                    .AddScoped<IAnswerSender, AnswerSender>()
                    .AddScoped<IGetterOfUnseenMessages, GetterOfUnseenMessages>()
                    .AddScoped<IHandlerOfAsSeenMarkerMessages, HandlerOfAsSeenMarkerMessages>()
                    .AddScoped<IClientConnector, ClientConnector>()

                    .AddScoped<IHandlerOfPublicIPGetterMessages, HandlerOfPublicIpGetterMessages>()
                    .AddScoped<IMessageGetter, MessageGetter>()

                    .AddScoped<IAsSeenMarkerProcessor, AsSeenMarkerProcessor>()
                    .AddScoped<IPublicIPGetterProcessor, PublicIPGetterProcessor>()

                    .AddScoped<ImapClient>();
                })
                .UseSerilog();
        }
    }
}
