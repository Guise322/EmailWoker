using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmailWorker.ApplicationCore.DomainServices
{
    public class EntryPointService : IEntryPointService
    {
        private readonly ILogger<EntryPointService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly EmailBoxServicesFactory _factoryOfServices; 
        
        public EntryPointService(ILogger<EntryPointService> logger,
            IServiceScopeFactory serviceScopeFactory,
            EmailBoxServicesFactory factoryOfServices) =>

        (_logger, _serviceScopeFactory, _factoryOfServices) = 
        (logger, serviceScopeFactory, factoryOfServices);
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Start execution at {Now}", DateTimeOffset.Now);

            List<IEmailBoxService> emailBoxServices =
                _factoryOfServices.CreateEmailBoxServices();

            foreach (var emailBoxService in emailBoxServices)
            {                
                ServiceStatus status = 
                    await emailBoxService.ProcessEmailInbox();
                _logger.LogInformation("ServiceWorkMessage", status.ServiceWorkMessage);
            }
        }
    }    
}