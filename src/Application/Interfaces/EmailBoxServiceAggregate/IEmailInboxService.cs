namespace EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;

internal interface IEmailInboxService
{
    Task<string> ProcessEmailInbox(EmailCredentials emailCredentials);
}