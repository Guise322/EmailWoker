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
        Assert.Throws<ArgumentNullException>(() => asSeenMarker.MarkAsSeen(null));
    }

    [Fact]
    public void MarkAsSeen_Messages_EmailData()
    {
        Mock<IImapClient> imapClientStub = new();
        imapClientStub.Setup(p => p.Inbox.Store(
            It.IsAny<UniqueId>(),
            It.IsAny<IStoreFlagsRequest>(),
            It.IsAny<CancellationToken>()
        )).Returns(true);
        int messageNumber = 2;
        var uniqueIDsShim = UniqueIDListShim.Create(messageNumber);
        AsSeenMarker asSeenMarker = new (imapClientStub.Object);
        EmailData actualEmailData = asSeenMarker.MarkAsSeen(uniqueIDsShim);
        Assert.Equal(
            ("Mark As Seen Service",
                "The count of messages marked as seen " + messageNumber.ToString()),
            (actualEmailData.EmailSubject, actualEmailData.EmailText)
        );
    }
}