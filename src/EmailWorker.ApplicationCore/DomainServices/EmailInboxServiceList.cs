using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;

namespace EmailWorker.ApplicationCore.DomainServices;

public class EmailBoxServiceList
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEmailCredentialsGetter _emailCredentialsGetter;
    public EmailBoxServiceList(
        IServiceProvider serviceProvider,
        IEmailCredentialsGetter emailCredentialsGetter
    ) =>
        (_serviceProvider, _emailCredentialsGetter) =
        (serviceProvider, emailCredentialsGetter);
    public List<IEmailInboxService> CreateEmailBoxServiceList()
    {   
        List<EmailCredentials> emailCredentialsList =
            _emailCredentialsGetter.GetEmailCredentials();
        List<IEmailInboxService> emailBoxServices = new();
        
        foreach (var emailCredentials in emailCredentialsList)
        {
            IEmailInboxService service = GetEmailBoxService(emailCredentials.DedicatedWork);
            service.EmailCredentials = emailCredentials;
            emailBoxServices.Add(service);
        }
        
        return emailBoxServices;
    }

    private IEmailInboxService GetEmailBoxService(DedicatedWorkType workType) =>
        workType switch
        {
            DedicatedWorkType.MarkAsSeen =>
                    _serviceProvider.GetRequiredService<IAsSeenMarkerService>(),
            DedicatedWorkType.SearchRequest =>
                    _serviceProvider.GetRequiredService<IPublicIPGetterService>(),
            _ => null
        };
}