using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.Tests.Unit.Shared;
using MailKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.Unit.ApplicationCore.PublicIPGetterServiceAggregate;

public class PublicIPGetterServiceUnitTest
{
    [Fact]
    public async Task ProcessEmailInbox_MessagesWithoutRequest_AppropriateServiceStatus()
    {
        Mock<IPublicIPGetter> publicIPGetterStub = new();
        Mock<IRequestMessageSearcher> requestMessageSearcherStub = new();
        Mock<IReportSender> reportSenderSrub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDListGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberAboveMinLimit = 1;

        unseenMessageIDListGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberAboveMinLimit));

        requestMessageSearcherStub.Setup(p => p
                .SearchRequestMessage(It.IsAny<IList<UniqueId>>(), It.IsAny<string>()))
            .Returns(new List<UniqueId>());

        PublicIPGetterService service = new(
            publicIPGetterStub.Object,
            requestMessageSearcherStub.Object,
            reportSenderSrub.Object,
            unseenMessageIDListGetterStub.Object,
            clientConnectorStub.Object
        );

        service.EmailCredentials = new EmailCredentials()
        {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await service.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
        { ServiceWorkMessage = 
            "The request is not found."
        };

        Assert.Equal(
            expectedServiceStatus.ServiceWorkMessage,
            actualServiceStatus.ServiceWorkMessage
        );
    }

    [Fact]
    public async Task ProcessEmailInbox_MessagesWithRequest_AppropriateServiceStatus()
    {
        Mock<IPublicIPGetter> publicIPGetterStub = new();
        Mock<IRequestMessageSearcher> requestMessageSearcherStub = new();
        Mock<IReportSender> reportSenderSrub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDListGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberAboveMinLimit = 1;

        unseenMessageIDListGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberAboveMinLimit));

        requestMessageSearcherStub.Setup(p => p
                .SearchRequestMessage(It.IsAny<IList<UniqueId>>(), It.IsAny<string>()))
            .Returns(new List<UniqueId>() { new UniqueId(1) });
        
        publicIPGetterStub.Setup(p => p.GetPublicIP(It.IsAny<List<UniqueId>>()))
            .Returns(new EmailData()
            { EmailSubject = "Ip By Email Service", EmailText = "111.111.111.111" }
        );
        
        PublicIPGetterService service = new(
            publicIPGetterStub.Object,
            requestMessageSearcherStub.Object,
            reportSenderSrub.Object,
            unseenMessageIDListGetterStub.Object,
            clientConnectorStub.Object
        );

        service.EmailCredentials = new EmailCredentials()
        {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await service.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
        { ServiceWorkMessage = "The request is detected. The current ip address is sent." };

        Assert.Equal(
            expectedServiceStatus.ServiceWorkMessage,
            actualServiceStatus.ServiceWorkMessage
        );
    }

    [Fact]
    public async Task ProcessEmailInbox_MessageWithWrongAutorFormat_AppropriateServiceStatus()
    {
        Mock<IPublicIPGetter> publicIPGetterStub = new();
        Mock<IRequestMessageSearcher> requestMessageSearcherStub = new();
        Mock<IReportSender> reportSenderSrub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDListGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberAboveMinLimit = 1;

        unseenMessageIDListGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberAboveMinLimit));

        requestMessageSearcherStub.Setup(p => p
                .SearchRequestMessage(It.IsAny<IList<UniqueId>>(), It.IsAny<string>()))
            .Throws(new FormatException());
        
        PublicIPGetterService service = new(
            publicIPGetterStub.Object,
            requestMessageSearcherStub.Object,
            reportSenderSrub.Object,
            unseenMessageIDListGetterStub.Object,
            clientConnectorStub.Object
        );

        service.EmailCredentials = new EmailCredentials()
        {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await service.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
        { ServiceWorkMessage = "The request message has the invalid format of its autor string." };

        Assert.Equal(
            expectedServiceStatus.ServiceWorkMessage,
            actualServiceStatus.ServiceWorkMessage
        );
    }

    [Fact]
    public async Task ProcessEmailInbox_MessageWithWrongAddressFormat_AppropriateServiceStatus()
    {
        Mock<IPublicIPGetter> publicIPGetterStub = new();
        Mock<IRequestMessageSearcher> requestMessageSearcherStub = new();
        Mock<IReportSender> reportSenderSrub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDListGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberAboveMinLimit = 1;

        unseenMessageIDListGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberAboveMinLimit));

        requestMessageSearcherStub.Setup(p => p
                .SearchRequestMessage(It.IsAny<IList<UniqueId>>(), It.IsAny<string>()))
            .Returns(new List<UniqueId>() { new UniqueId(1) });
        
        publicIPGetterStub.Setup(p => p
                .GetPublicIP(It.IsAny<List<UniqueId>>()))
            .Throws(new ArgumentException("The response message is in invalid format."));
        
        PublicIPGetterService service = new(
            publicIPGetterStub.Object,
            requestMessageSearcherStub.Object,
            reportSenderSrub.Object,
            unseenMessageIDListGetterStub.Object,
            clientConnectorStub.Object
        );

        service.EmailCredentials = new EmailCredentials()
        {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await service.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
        { ServiceWorkMessage = "The response message has no or invalid address string." };

        Assert.Equal(
            expectedServiceStatus.ServiceWorkMessage,
            actualServiceStatus.ServiceWorkMessage
        );
    }

    [Fact]
    public async Task ProcessEmailInbox_MessageWithEmptyAddress_AppropriateServiceStatus()
    {
        Mock<IPublicIPGetter> publicIPGetterStub = new();
        Mock<IRequestMessageSearcher> requestMessageSearcherStub = new();
        Mock<IReportSender> reportSenderSrub = new();
        Mock<IUnseenMessageIDListGetter> unseenMessageIDListGetterStub = new();
        Mock<IClientConnector> clientConnectorStub = new();

        int messageNumberAboveMinLimit = 1;

        unseenMessageIDListGetterStub.Setup(p => p
                .GetUnseenMessageIDsAsync(It.IsAny<EmailCredentials>()))
            .ReturnsAsync(UniqueIDList.Create(messageNumberAboveMinLimit));

        requestMessageSearcherStub.Setup(p => p
                .SearchRequestMessage(It.IsAny<IList<UniqueId>>(), It.IsAny<string>()))
            .Returns(new List<UniqueId>() { new UniqueId(1) });
        
        publicIPGetterStub.Setup(p => p
                .GetPublicIP(It.IsAny<List<UniqueId>>()))
            .Throws(new ArgumentException("The response message has no address string"));
        
        PublicIPGetterService service = new(
            publicIPGetterStub.Object,
            requestMessageSearcherStub.Object,
            reportSenderSrub.Object,
            unseenMessageIDListGetterStub.Object,
            clientConnectorStub.Object
        );

        service.EmailCredentials = new EmailCredentials()
        {
            MailServer = "test",
            Port = 111,
            Ssl = true,
            Login = "test",
            Password = "test",
            DedicatedWork = DedicatedWorkType.MarkAsSeen
        };

        ServiceStatus actualServiceStatus = await service.ProcessEmailInbox();

        ServiceStatus expectedServiceStatus = new()
        { ServiceWorkMessage = "The response message has no or invalid address string." };

        Assert.Equal(
            expectedServiceStatus.ServiceWorkMessage,
            actualServiceStatus.ServiceWorkMessage
        );
    }
}