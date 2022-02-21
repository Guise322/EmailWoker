using Xunit;
using Moq;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices;
using EmailWorker.Infrastructure;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
using MailKit.Net.Imap;
using MailKit;
using System;

namespace EmailWorker.Tests.UnitTests.ApplicationCore;

public class CreateEmailBoxServicesUnitTest
{
    [Fact]
    public void CreateEmailBoxServices_TwoEmailCredentials_TwoEmailBoxServices()
    {
        Mock<IEmailCredentialsGetter> emailCredentialsGetterStub = new();
        emailCredentialsGetterStub.Setup(p => p.GetEmailCredentials()).Returns(
            new List<EmailCredentials>()
            {
                new EmailCredentials() { DedicatedWork = DedicatedWorkType.MarkAsSeen},
                new EmailCredentials() { DedicatedWork = DedicatedWorkType.SearchRequest}
            }
        );

        Mock<IAsSeenMarkerService> asSeenMarkerServiceStub = new();
        Mock<IPublicIPGetterService> publicIPGetterServiceStub = new();

        var services = new ServiceCollection();
        services.AddTransient(s => asSeenMarkerServiceStub.Object);
        services.AddTransient(s => publicIPGetterServiceStub.Object);

        ServiceProvider provider = services.BuildServiceProvider();
        
        EmailBoxServicesFactory emailBoxServiceDataFactoryList = new(
                provider,
                emailCredentialsGetterStub.Object
            );
        List<IEmailBoxService> actualEmailBoxDataServiceList =
            emailBoxServiceDataFactoryList.CreateEmailBoxServices();

        Assert.Equal(asSeenMarkerServiceStub.Object, actualEmailBoxDataServiceList[0]);
        Assert.Equal(publicIPGetterServiceStub.Object, actualEmailBoxDataServiceList[1]);
    }
}