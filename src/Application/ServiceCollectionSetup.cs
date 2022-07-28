using EmailWorker.Application.Interfaces;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Application;

public static class ServiceCollectionSetup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services.AddSingleton<IEmailInboxServiceCommand, EmailInboxServiceCommand>()
            .AddSingleton<IEmailInboxServiceFactory, EmailInboxServiceFactory>()
            .AddSingleton<IAsSeenMarkerService, AsSeenMarkerService>()
            .AddSingleton<IPublicIPGetterService, PublicIPGetterService>();
}