using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using Microsoft.Extensions.Logging;

namespace EmailWorker.ApplicationCore.DomainServices;
public class EntryPointService : IEntryPointService
{
    private readonly ILogger<EntryPointService> _logger;
    private readonly EmailInboxServiceList _emailInboxServiceList; 
    
    public EntryPointService(
        ILogger<EntryPointService> logger,
        EmailInboxServiceList emailBoxServiceList
    ) =>
        (_logger, _emailInboxServiceList) = 
        (logger, emailBoxServiceList);
    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Start execution at {Now}", DateTimeOffset.Now);

        List<IEmailInboxService> emailInboxServices =
            _emailInboxServiceList.CreateEmailInboxServiceList();

        foreach (var emailInboxService in emailInboxServices)
        {                
            ServiceStatus status = 
                await emailInboxService.ProcessEmailInbox();
            _logger.LogInformation("{ServiceWorkMessage}", status.ServiceWorkMessage);
        }
    }
}