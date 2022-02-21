using EmailWorker.ApplicationCore.Entities;
using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
using EmailWorker.Tests.UnitTests.Shared;
using MailKit;
using MailKit.Net.Imap;
using Moq;
using System;
using Xunit;

namespace EmailWorker.Tests.UnitTests.Infrastructure;

public class HandlerOfAsSeenMarkerMessagesUnitTest
{
    [Fact]
    public void HandleProcessedMessages_Null_ThrowsArgumentNullException()
    {
        Mock<ImapClient> mailStoreStub = new();
        HandlerOfAsSeenMarkerMessages handler = new(mailStoreStub.Object);
        var ex = Record.Exception(() => handler.HandleProcessedMessages(null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void HandlerProcessedMessages_NumberOfMessagesIDsAboveMaxLimit_NullNull()
    {
        Mock<ImapClient> mailStoreStub = new();
        HandlerOfAsSeenMarkerMessages handler = new(mailStoreStub.Object);
        Mock<IMailFolder> inboxStub = new();
        mailStoreStub.SetupGet(p => p.Inbox).Returns(inboxStub.Object);
        int numberAboveMaxLimit = 1000;
        var uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        EmailData actualEmailData = handler.HandleProcessedMessages(uniqueIDsShim);
        Assert.Null(actualEmailData);
    }

    [Fact]
    public void HandlerProcessedMessages_NumberOfMessageIDsBelowMaxLimit_NumberOfProcessedMessaggesAndInfoString()
    {
        Mock<ImapClient> mailStoreStub = new();
        HandlerOfAsSeenMarkerMessages handler = new(mailStoreStub.Object);
        Mock<IMailFolder> inboxStub = new();
        mailStoreStub.SetupGet(p => p.Inbox).Returns(inboxStub.Object);
        int numberAboveMaxLimit = 999;
        var uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        EmailData actualEmailData = handler.HandleProcessedMessages(uniqueIDsShim);
        Assert.Equal(
            ("The count of messages marked as seen", numberAboveMaxLimit.ToString()),
            (actualEmailData.EmailSubject, actualEmailData.EmailText));
    }
}