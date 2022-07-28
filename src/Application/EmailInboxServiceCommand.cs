using EmailWorker.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Application;

internal class EmailInboxServiceCommand : IEmailInboxServiceCommand
{
    private readonly ILogger<EmailInboxServiceCommand> _logger;
    private readonly IEmailCredentialsGetter _emailCredentialsGetter;
    private readonly IEmailInboxServiceFactory _emailInboxServiceFactory;

    public EmailInboxServiceCommand(
        ILogger<EmailInboxServiceCommand> logger,
        IEmailCredentialsGetter emailCredentialsGetter,
        IEmailInboxServiceFactory emailBoxServiceFactory
    )
    {
        _logger = logger;
        _emailCredentialsGetter = emailCredentialsGetter;
        _emailInboxServiceFactory = emailBoxServiceFactory;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Start execution at {Now}", DateTimeOffset.Now);

        List<EmailCredentials>? emailCredentialsList = _emailCredentialsGetter.GetEmailCredentialsList();

        if (emailCredentialsList is null)
        {
            _logger.LogInformation("Cannot get email credentials from the file");

            return;
        }

        foreach (var emailCredentials in emailCredentialsList)
        {
            var emailBoxService =
                _emailInboxServiceFactory.CreateEmailInboxService(emailCredentials.DedicatedWork);
            string status = await emailBoxService.ProcessEmailInbox(emailCredentials);
            _logger.LogInformation("{ServiceWorkMessage}", status);
        }
    }
}