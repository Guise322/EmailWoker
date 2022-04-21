using EmailWorker.ApplicationCore.Entities;
using EmailWorker.Infrastructure;
using EmailWorker.Tests.UnitTests.Shared;
using MailKit;
using MailKit.Net.Imap;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace EmailWorker.Tests.UnitTests.Infrastructure;

public class AsSeenMaarkerUnitTest
{
    [Fact]
    public void MarkAsSeen_Null_ThrowsArgumentNullException()
    {
        Mock<IImapClient> imapClientStub = new();
        AsSeenMarker asSeenMarker = new(imapClientStub.Object);
        var exception = Record.Exception(() => asSeenMarker.MarkAsSeen(null));
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void MarkAsSeen_Messages_EmailData()
    {
        Mock<IImapClient> imapClientShim = new();
        imapClientShim.Setup(p => p.Inbox.Store(
            It.IsAny<UniqueId>(),
            It.IsAny<IStoreFlagsRequest>(),
            It.IsAny<CancellationToken>()
        )).Returns(true);
        int messageNumber = 2;
        var uniqueIDListShim = UniqueIDListShim.Create(messageNumber);
        AsSeenMarker asSeenMarker = new (imapClientShim.Object);
        EmailData actualEmailData = asSeenMarker.MarkAsSeen(uniqueIDListShim);
        Assert.Equal(
            ("Mark As Seen Service",
                "The count of messages marked as seen " + messageNumber.ToString()),
            (actualEmailData.EmailSubject, actualEmailData.EmailText)
        );
    }
}