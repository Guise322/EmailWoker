using System;
using EmailWorker.ApplicationCore.DomainServices;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.ApplicationCore;

public static class ServiceCollectionSetup
{
    public static IServiceCollection AddApplicationCoreServices(this IServiceCollection services) =>
        services.AddTransient<IEntryPointService, EntryPointService>()
            .AddTransient<EmailBoxServiceList>()
            .AddTransient<IAsSeenMarkerService, AsSeenMarkerService>()
            .AddTransient<IPublicIPGetterService, PublicIPGetterService>()
            .AddTransient<IRequestMessageSearcher, RequestMessageSearcher>();
}