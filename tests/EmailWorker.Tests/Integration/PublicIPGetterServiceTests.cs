using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.Application;
using EmailWorker.Application.Interfaces;
using MailKit;
using MimeKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.Integration;

public class PublicIPGetterServiceUnitTests
{
    [Theory]
    [InlineData(2, 0)]
    public async Task ProcessEmailInbox_MessagesWithoutRequest_ServiceStatus(
        int messageCount, int requestCount)
    {
        var clientConnectorStub = new Mock<IClientConnector>();
        var unseenMessageIdGetterStub = new Mock<IUnseenMessageIdGetter>();
        var messageGetterStub = new Mock<IMessageGetter>();
        var asSeenMarkerStub = new Mock<IAsSeenMarker>();
        var publicIPGetterStub = new Mock<IPublicIPGetter>();
        var reportSenderSrub = new Mock<IReportSender>();
        

        unseenMessageIdGetterStub.Setup(o => o
                .GetUnseenMessageIds())
            .ReturnsAsync(UniqueIdFactory.Create(messageCount));
        
        int r = 0;
        
        messageGetterStub.Setup(o => o.GetMessage(It.IsAny<UniqueId>()))
            .Returns(() => 
            {
                if (r < requestCount)
                {
                    r++;
                    return new MimeMessage(
                        new List<MailboxAddress>() { new MailboxAddress("test", "guise322@ya.ru") },
                        new List<MailboxAddress>() { new MailboxAddress("test", "test@mail.com") },
                        "test",
                        new MimePart()
                    );
                }

                return new MimeMessage(
                    new List<MailboxAddress>() { new MailboxAddress("test", "test1@mail.ru") },
                    new List<MailboxAddress>() { new MailboxAddress("test", "test@mail.com") },
                    "test",
                    new MimePart()
                );

            });

        var service = new PublicIPGetterService(
            clientConnectorStub.Object,
            unseenMessageIdGetterStub.Object,
            messageGetterStub.Object,
            asSeenMarkerStub.Object,
            publicIPGetterStub.Object,
            reportSenderSrub.Object
        );

        var emailCredentials = TestEmailCredentialsFactory.Create();

        string actualResult = await service.ProcessEmailInbox(emailCredentials);
        string expectedResult = "A request is not found";
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    public async Task ProcessEmailInbox_MessagesWithRequests_ServiceStatus(
        int messageCount, int requestCount)
    {
        var clientConnectorStub = new Mock<IClientConnector>();
        var unseenMessageIdGetterStub = new Mock<IUnseenMessageIdGetter>();
        var messageGetterStub = new Mock<IMessageGetter>();
        var asSeenMarkerStub = new Mock<IAsSeenMarker>();
        var publicIPGetterStub = new Mock<IPublicIPGetter>();
        var reportSenderSrub = new Mock<IReportSender>();
        

        unseenMessageIdGetterStub.Setup(o => o
                .GetUnseenMessageIds())
            .ReturnsAsync(UniqueIdFactory.Create(messageCount));
        
        int r = 0;
        
        messageGetterStub.Setup(o => o.GetMessage(It.IsAny<UniqueId>()))
            .Returns(() => 
            {
                if (r < requestCount)
                {
                    r++;
                    return new MimeMessage(
                        new List<MailboxAddress>() { new MailboxAddress("test", "guise322@ya.ru") },
                        new List<MailboxAddress>() { new MailboxAddress("test", "test@mail.com") },
                        "test",
                        new MimePart()
                    );
                }

                return new MimeMessage(
                    new List<MailboxAddress>() { new MailboxAddress("test", "test1@mail.ru") },
                    new List<MailboxAddress>() { new MailboxAddress("test", "test@mail.com") },
                    "test",
                    new MimePart()
                );

            });

        publicIPGetterStub.Setup(o => o.GetPublicIP())
            .Returns(new EmailData()
                {
                    EmailSubject = "Ip By Email Service",
                    EmailText = "111.111.111.111"
                });

        var service = new PublicIPGetterService(
            clientConnectorStub.Object,
            unseenMessageIdGetterStub.Object,
            messageGetterStub.Object,
            asSeenMarkerStub.Object,
            publicIPGetterStub.Object,
            reportSenderSrub.Object
        );

        var emailCredentials = TestEmailCredentialsFactory.Create();

        string actualResult = await service.ProcessEmailInbox(emailCredentials);
        string expectedResult = "A request is detected, and the current IP address is sent";
        Assert.Equal(expectedResult, actualResult);
    }
}