namespace EmailWorker.Application.Interfaces;

public interface IEmailInboxServiceCommand
{
    Task ExecuteAsync(CancellationToken stoppingToken);
}