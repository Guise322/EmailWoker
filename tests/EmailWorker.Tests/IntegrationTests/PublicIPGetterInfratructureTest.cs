using Xunit;
using Moq;
using System.Net.Http;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.Infrastructure;
using MailKit.Net.Imap;
using System.Collections.Generic;
using MailKit;
using EmailWorker.Tests.UnitTests.Shared;
using System.Threading;
using System;

namespace EmailWorker.Tests.IntegrationTests;

public class PublicIPGetterIntegrationTest
{
    [Fact]
    public void GetPublicIP_UniqueIDList_IPAddressAndInformationMessage()
    {
        Mock<IHttpClientFactory> factoryStub = new();
        factoryStub.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient());

        Mock<IImapClient> imapClientStub = new();
        imapClientStub.Setup(p => p.Inbox.Store(
            It.IsAny<UniqueId>(),
            It.IsAny<IStoreFlagsRequest>(),
            It.IsAny<CancellationToken>()
        )).Returns(true);
        List<UniqueId> uniqueIDListShim = UniqueIDListShim.Create(2);

        PublicIPGetter publicIPGetter = new(factoryStub.Object, imapClientStub.Object);
        EmailData actualEmailData = publicIPGetter.GetPublicIP(uniqueIDListShim);
        Assert.Matches(@"\d", actualEmailData.EmailText);
    }

    [Fact]
    public void GetPublicIP_Null_ThrowsArgumentNullException()
    {
        Mock<IHttpClientFactory> factoryStub = new();
        Mock<IImapClient> imapClientStub = new();
        PublicIPGetter publicIPGetter = new(factoryStub.Object, imapClientStub.Object);
        Assert.Throws<ArgumentNullException>(() => publicIPGetter.GetPublicIP(null));
    }
}