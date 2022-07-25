using EmailWorker.Application.Enums;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;

namespace EmailWorker.Application.Interfaces;

internal interface IEmailInboxServiceFactory
{
    IEmailInboxService CreateEmailInboxService(DedicatedWorkType workType);
}