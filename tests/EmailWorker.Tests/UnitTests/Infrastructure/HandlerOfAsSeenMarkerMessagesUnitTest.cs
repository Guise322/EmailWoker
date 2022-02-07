using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
using EmailWorker.Tests.UnitTests.Shared;
using MailKit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace EmailWorker.Tests.UnitTests.Infrastructure;

public class HandlerOfAsSeenMarkerMessagesUnitTest
{
    [Fact]
    public void HandleProcessedMessages_Null_ThrowsArgumentNullException()
    {
        Mock<ILogger<HandlerOfAsSeenMarkerMessages>> loggerStub = new();
        Mock<IMailStore> mailStoreStub = new();
        HandlerOfAsSeenMarkerMessages handler = new(loggerStub.Object, mailStoreStub.Object);
        var ex = Record.Exception(() => handler.HandleProcessedMessages(null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void HandlerProcessedMessages_NumberOfMessagesIDsAboveMaxLimit_NullNull()
    {
        Mock<ILogger<HandlerOfAsSeenMarkerMessages>> loggerStub = new();
        Mock<IMailStore> mailStoreStub = new();
        HandlerOfAsSeenMarkerMessages handler = new(loggerStub.Object, mailStoreStub.Object);
        Mock<IMailFolder> inboxStub = new();
        mailStoreStub.SetupGet(p => p.Inbox).Returns(inboxStub.Object);
        int numberAboveMaxLimit = 1000;
        var uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        (var actualEmailText, var actualEmailSubject) = handler.HandleProcessedMessages(uniqueIDsShim);
        Assert.Equal((null, null),(actualEmailText, actualEmailSubject));
    }

    [Fact]
    public void HandlerProcessedMessages_NumberOfMessageIDsBelowMaxLimit_NumberOfProcessedMessaggesAndInfoString()
    {
        Mock<ILogger<HandlerOfAsSeenMarkerMessages>> loggerStub = new();
        Mock<IMailStore> mailStoreStub = new();
        HandlerOfAsSeenMarkerMessages handler = new(loggerStub.Object, mailStoreStub.Object);
        Mock<IMailFolder> inboxStub = new();
        mailStoreStub.SetupGet(p => p.Inbox).Returns(inboxStub.Object);
        int numberAboveMaxLimit = 999;
        var uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        (var actualEmailText, var actualEmailSubject) = handler.HandleProcessedMessages(uniqueIDsShim);
        Assert.Equal(
            (numberAboveMaxLimit.ToString(),"The count of messages marked as seen"),
            (actualEmailText, actualEmailSubject));
    }
}