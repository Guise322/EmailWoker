using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Imap;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Infrastructure;

public static class ServiceCollectionSetup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services.AddTransient<IEmailCredentialsGetter, EmailCredentialsGetter>()

            .AddScoped<IReportSender, ReportSender>()

            .AddScoped<IUnseenMessageIDListGetter, UnseenMessageIDListGetter>()
            .AddScoped<IAsSeenMarker, AsSeenMarker>()
            .AddScoped<IClientConnector, ClientConnector>()

            .AddScoped<IPublicIPGetter, PublicIPGetter>()
            .AddScoped<IMessageGetter, MessageGetter>()
                    
            .AddTransient<IImapClient, ImapClient>();
}