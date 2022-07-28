using System.Collections.ObjectModel;

namespace EmailWorker.Application.Interfaces;

public interface IEmailCredentialsGetter
{
    Task<ReadOnlyCollection<EmailCredentials>?> GetEmailCredentialsCollection(CancellationToken stoppingToken);
}