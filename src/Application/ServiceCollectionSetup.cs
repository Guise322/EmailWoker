using System;
using EmailWorker.Application.Interfaces;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Application;

public static class ServiceCollectionSetup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
        services.AddScoped<IEmailInboxServiceCommand, EmailInboxServiceCommand>()
            .AddScoped<IEmailInboxServiceFactory, EmailInboxServiceFactory>()
            .AddScoped<IAsSeenMarkerService, AsSeenMarkerService>()
            .AddScoped<IPublicIPGetterService, PublicIPGetterService>();
}