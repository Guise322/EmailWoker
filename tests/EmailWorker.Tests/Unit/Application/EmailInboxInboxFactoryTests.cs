using Xunit;
using Moq;
using EmailWorker.Application.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using EmailWorker.Application;
using System;

namespace EmailWorker.Tests.Unit.Application;

public class EmailInboxServiceListTests
{
    private static List<DedicatedWorkType> CreateWorkTypes() =>
        new()
        {
            DedicatedWorkType.MarkAsSeen,
            DedicatedWorkType.SearchRequest
        };

    private static void AddStubServices(IServiceCollection services)
    {
        services.AddSingleton(s => new Mock<IAsSeenMarkerService>().Object)
            .AddSingleton(s => new Mock<IPublicIPGetterService>().Object);
    }

    [Fact]
    public void CreateEmailInboxService_EmailCredentials_EmailInboxServices()
    {
        IServiceCollection services = new ServiceCollection();

        AddStubServices(services);

        ServiceProvider providerShim = services.BuildServiceProvider();

        var emailInboxServiceFactoryStub = new EmailInboxServiceFactory(providerShim);
        var actualEmailInboxServices = new List<IEmailInboxService>();

        var workTypes = CreateWorkTypes();

        foreach (var workType in workTypes)
        {
            actualEmailInboxServices.Add(
                emailInboxServiceFactoryStub.CreateEmailInboxService(workType));
        }

        Assert.Equal(workTypes.Count, actualEmailInboxServices.Count);
    }

    [Fact]
    public void CreateEmailInboxService_WrongWorkType_ArgumentException()
    {
        var services = new ServiceCollection();

        AddStubServices(services);

        ServiceProvider providerShim = services.BuildServiceProvider();

        var emailInboxServiceFactoryStub = new EmailInboxServiceFactory(providerShim);
        var actualEmailInboxServices = new List<IEmailInboxService>();

        var actualException = Record.Exception(() =>
            actualEmailInboxServices.Add(emailInboxServiceFactoryStub.CreateEmailInboxService((DedicatedWorkType)2)));

        Assert.IsType<ArgumentException>(actualException);
    }
}