using EmailWorker.Application.Interfaces;
using MailKit.Net.Imap;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Infrastructure;

public static class ServiceCollectionSetup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services.AddHttpClient()
            
            .AddScoped<IEmailCredentialsGetter, EmailCredentialsGetter>()

            .AddScoped<IReportSender, ReportSender>()

            .AddScoped<IUnseenMessageIdGetter, UnseenMessageIdGetter>()
            .AddScoped<IAsSeenMarker, AsSeenMarker>()
            .AddScoped<IClientConnector, ClientConnector>()

            .AddScoped<IPublicIPGetter, PublicIPGetter>()
            .AddScoped<IMessageGetter, MessageGetter>()
                    
            .AddScoped<IImapClient, ImapClient>();
}