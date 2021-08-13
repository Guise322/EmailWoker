using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.Infrastructure.EmailProcessor;
using EmailWorker.Infrastructure.EmailProcessor.HandlersOfProcessedMessages;
using EmailWorker.Infrastructure.EmailProcessor.GetterOfUnseenMessages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;

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
                        .AddTransient<IAnswerSender, AnswerSender>()
                        .AddTransient<IGetterOfUnseenMessages, InboxGetter>()
                        .AddTransient<IHandlerOfAsSeenMarkerMessages, HandlerOfAsSeenMarkerMessages>()
                        .AddTransient<IClientConnector, ClientConnector>()

                        .AddTransient<IHandlerOfPublicIPGetterMessages, HandlerOfPublicIpGetterMessages>()
                        .AddTransient<IMessageGetter, MessageGetter>()
                        
                        ;
                });
    }
}
