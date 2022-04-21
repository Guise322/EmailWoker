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
        Mock<IHttpClientFactory> httpClientFactoryShim = new();
        httpClientFactoryShim.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient());

        Mock<IImapClient> imapClientShim = new();
        imapClientShim.Setup(p => p.Inbox.Store(
            It.IsAny<UniqueId>(),
            It.IsAny<IStoreFlagsRequest>(),
            It.IsAny<CancellationToken>()
        )).Returns(true);
        List<UniqueId> uniqueIDListShim = UniqueIDListShim.Create(2);

        PublicIPGetter publicIPGetter = new(httpClientFactoryShim.Object, imapClientShim.Object);
        EmailData actualEmailData = publicIPGetter.GetPublicIP(uniqueIDListShim);
        Assert.Matches(@"\d", actualEmailData.EmailText);
    }

    [Fact]
    public void GetPublicIP_Null_ThrowsArgumentNullException()
    {
        Mock<IHttpClientFactory> httpClientFactoryShim = new();
        Mock<IImapClient> imapClientShim = new();
        PublicIPGetter publicIPGetter = new(httpClientFactoryShim.Object, imapClientShim.Object);
        var exception = Record.Exception(() => publicIPGetter.GetPublicIP(null));
        Assert.IsType<NullReferenceException>(exception);
    }
}