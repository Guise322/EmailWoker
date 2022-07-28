using EmailWorker.Application.Interfaces;
using MailKit.Net.Imap;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Infrastructure;

public static class ServiceCollectionSetup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services.AddHttpClient()

            .AddSingleton<IEmailCredentialsGetter, EmailCredentialsGetter>()

            .AddSingleton<IReportSender, ReportSender>()

            .AddSingleton<IUnseenMessageIdGetter, UnseenMessageIdGetter>()
            .AddSingleton<IAsSeenMarker, AsSeenMarker>()
            .AddSingleton<IClientConnector, ClientConnector>()

            .AddSingleton<IPublicIPGetter, PublicIPGetter>()
            .AddSingleton<IMessageGetter, MessageGetter>()

            .AddSingleton<IImapClient, ImapClient>();
}