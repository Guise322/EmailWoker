using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using Microsoft.Extensions.Logging;

namespace EmailWorker.ApplicationCore.DomainServices
{
    public class EntryPointService : IEntryPointService
    {
        private readonly ILogger<EntryPointService> _logger;
        private readonly EmailBoxServiceList _emailBoxServiceList; 
        
        public EntryPointService(
            ILogger<EntryPointService> logger,
            EmailBoxServiceList emailBoxServiceList
        ) =>
            (_logger, _emailBoxServiceList) = 
            (logger, emailBoxServiceList);
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Start execution at {Now}", DateTimeOffset.Now);

            List<IEmailInboxService> emailBoxServices =
                _emailBoxServiceList.CreateEmailBoxServiceList();

            foreach (var emailBoxService in emailBoxServices)
            {                
                ServiceStatus status = 
                    await emailBoxService.ProcessEmailInbox();
                _logger.LogInformation("{ServiceWorkMessage}", status.ServiceWorkMessage);
            }
        }
    }    
}