using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.Application;
using EmailWorker.Application.Interfaces;
using MailKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.Integration;

public class AsSeenMarkerServiceTests
{
    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    public async Task ProcessEmailInbox_MessageNumberBelowLimit_ServiceStatus(int messageNumber)
    {
        var asSeenMarkerStub = new Mock<IAsSeenMarker>();
        var clientConnectorStub = new Mock<IClientConnector>();
        var unseenMessageIdGetterStub = new Mock<IUnseenMessageIdGetter>();
        var reportSenderStub = new Mock<IReportSender>();

        unseenMessageIdGetterStub.Setup(o => o
                .GetUnseenMessageIds())
            .ReturnsAsync(UniqueIdFactory.Create(messageNumber));
        var emailCredentials = TestEmailCredentialsFactory.Create();

        var markerService = new AsSeenMarkerService(
            asSeenMarkerStub.Object,
            clientConnectorStub.Object,
            unseenMessageIdGetterStub.Object,
            reportSenderStub.Object
        );

        string actualResult = await markerService.ProcessEmailInbox(emailCredentials);
        string expectedResult = "The given number of messages is too small";
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(6)]
    public async Task ProcessEmailInbox_MessageNumberAboveLimit_ServiceStatus(int messageNumber)
    {
        var asSeenMarkerStub = new Mock<IAsSeenMarker>();
        var clientConnectorStub = new Mock<IClientConnector>();
        var unseenMessageIdGetterStub = new Mock<IUnseenMessageIdGetter>();
        var reportSenderStub = new Mock<IReportSender>();

        List<UniqueId> uniqueIds = UniqueIdFactory.Create(messageNumber);

        unseenMessageIdGetterStub.Setup(o => o
                .GetUnseenMessageIds())
            .ReturnsAsync(uniqueIds);
        asSeenMarkerStub.Setup(o => o.MarkAsSeen(uniqueIds))
            .Returns(new EmailData(EmailSubject: "test", EmailText: "test"));
        var emailCredentials = TestEmailCredentialsFactory.Create();

        var markerService = new AsSeenMarkerService(
            asSeenMarkerStub.Object,
            clientConnectorStub.Object,
            unseenMessageIdGetterStub.Object,
            reportSenderStub.Object
        );

        string actualResult = await markerService.ProcessEmailInbox(emailCredentials);
        string expectedResult = "All messages is marked as seen";
        Assert.Equal(expectedResult, actualResult);
    }
}