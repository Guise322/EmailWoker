using EmailWorker.Application.Enums;
using EmailWorker.Application.Interfaces;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using Microsoft.Extensions.DependencyInjection;

namespace EmailWorker.Application;

internal sealed class EmailInboxServiceFactory : IEmailInboxServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public EmailInboxServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEmailInboxService CreateEmailInboxService(DedicatedWorkType workType) =>
        workType switch
        {
            DedicatedWorkType.MarkAsSeen =>
                    _serviceProvider.GetRequiredService<IAsSeenMarkerService>(),
            DedicatedWorkType.SearchRequest =>
                    _serviceProvider.GetRequiredService<IPublicIPGetterService>(),
            _ => throw new ArgumentException("A wrong work type!")
        };
}