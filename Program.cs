using EmailWorker.ApplicationCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxProcessorAggregate;
using EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.AsSeenMarkerAggregate;
using EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.PublicIPGetterAggregate;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessorService;
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
                        .AddTransient<IEmailBoxProcessorService, EmailBoxProcessorService>() 

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
