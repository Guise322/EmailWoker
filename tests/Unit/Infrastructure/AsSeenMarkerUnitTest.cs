using EmailWorker.Application;
using EmailWorker.Infrastructure;
using MailKit;
using MailKit.Net.Imap;
using Moq;
using System.Threading;
using Xunit;

namespace EmailWorker.Tests.Unit.Infrastructure;

public class AsSeenMaarkerUnitTest
{
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
        var uniqueIDListShim = UniqueIDFactory.Create(messageNumber);
        var asSeenMarker = new AsSeenMarker(imapClientShim.Object);
        EmailData actualEmailData = asSeenMarker.MarkAsSeen(uniqueIDListShim);
        Assert.Equal(
            ("Mark As Seen Service",
                "The count of messages marked as seen " + messageNumber.ToString()),
            (actualEmailData.EmailSubject, actualEmailData.EmailText)
        );
    }
}