using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.Tests.Unit.Shared;
using MailKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.Unit.ApplicationCore.AsSeenMarkerServiceAggregate;

public class AsSeenMarkerServiceUnitTest
{
    [Fact]
    public async Task ProcessEmailInbox_EmailCredentialsIsNull_ArgumentNullException()
    {
        Mock<IAsSeenMarker> asSeenMarkerStub = new();
        Mock<IReportSender> reportSenderStub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDistGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        AsSeenMarkerService markerService = new(
            asSeenMarkerStub.Object,
            reportSenderStub.Object,
            unseenMessageIDistGetterStub.Object,
            clientConnectorStub.Object
        );
    
        var exception = await Record.ExceptionAsync(() => markerService.ProcessEmailInbox());
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public async Task ProcessEmailInbox_MessageNumberBelowMinLimit_AppropriateServiceStatus()
    {
        Mock<IAsSeenMarker> asSeenMarkerStub = new();
        Mock<IReportSender> reportSenderStub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDistGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberBelowMinLimit = 1;

        unseenMessageIDistGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberBelowMinLimit));

        AsSeenMarkerService markerService = new(
            asSeenMarkerStub.Object,
            reportSenderStub.Object,
            unseenMessageIDistGetterStub.Object,
            clientConnectorStub.Object
        );

        markerService.EmailCredentials = new EmailCredentials() {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await markerService.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
            { ServiceWorkMessage = "The given number of messages is too small." };

        Assert.Equal(expectedServiceStatus.ServiceWorkMessage, actualServiceStatus.ServiceWorkMessage);
    }

    [Fact]
    public async Task ProcessEmailInbox_MessageNumberAboveMinLimit_AppropriateServiceStatus()
    {
        Mock<IAsSeenMarker> asSeenMarkerStub = new();
        Mock<IReportSender> reportSenderStub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDListGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberAboveMinLimit = 5;

        unseenMessageIDListGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberAboveMinLimit));

        asSeenMarkerStub.Setup(p => p.MarkAsSeen(It.IsAny<List<UniqueId>>()))
            .Returns(new EmailData() { EmailSubject = "test", EmailText = "test" });

        AsSeenMarkerService markerService = new(
            asSeenMarkerStub.Object,
            reportSenderStub.Object,
            unseenMessageIDListGetterStub.Object,
            clientConnectorStub.Object
        );

        markerService.EmailCredentials = new EmailCredentials() {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await markerService.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
            { ServiceWorkMessage = "All messages is marked as seen." };

        Assert.Equal(expectedServiceStatus.ServiceWorkMessage, actualServiceStatus.ServiceWorkMessage);
    }
}