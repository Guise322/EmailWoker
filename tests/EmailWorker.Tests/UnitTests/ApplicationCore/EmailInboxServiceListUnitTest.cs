using Xunit;
using Moq;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using EmailWorker.ApplicationCore.DomainServices;

namespace EmailWorker.Tests.UnitTests.ApplicationCore;

public class EmailInboxServiceListUnitTest
{
    [Fact]
    public void CreateEmailInboxServiceList_TwoEmailCredentials_TwoEmailInboxServices()
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

        ServiceProvider providerShim = services.BuildServiceProvider();
        
        EmailInboxServiceList emailBoxServiceListStub = new(
                providerShim,
                emailCredentialsGetterStub.Object
            );
        List<IEmailInboxService> actualEmailInboxServiceList =
            emailBoxServiceListStub.CreateEmailInboxServiceList();

        Assert.Equal(asSeenMarkerServiceStub.Object, actualEmailInboxServiceList[0]);
        Assert.Equal(publicIPGetterServiceStub.Object, actualEmailInboxServiceList[1]);
    }
}