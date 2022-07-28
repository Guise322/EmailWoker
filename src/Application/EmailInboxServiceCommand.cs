using System.Collections.ObjectModel;
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

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start execution at {Now}", DateTimeOffset.Now);

        ReadOnlyCollection<EmailCredentials>? emailCredentialsCollection =
            await _emailCredentialsGetter.GetEmailCredentialsCollection(stoppingToken);

        if (emailCredentialsCollection is null)
        {
            _logger.LogError("Cannot get email credentials from the file");

            return;
        }

        foreach (var emailCredentials in emailCredentialsCollection)
        {
            if (!EmailCredentialsValidator.Validate(emailCredentials))
            {
                _logger.LogError("Not all data presented in the email credentials file");

                return;
            }

            var emailBoxService =
                _emailInboxServiceFactory.CreateEmailInboxService(emailCredentials.DedicatedWork);
            string status = await emailBoxService.ProcessEmailInbox(emailCredentials);
            _logger.LogInformation("{ServiceWorkMessage}", status);
        }
    }
}