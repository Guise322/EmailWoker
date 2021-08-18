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

namespace EmailWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
                });
    }
}
