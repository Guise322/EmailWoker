using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
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

    }

    [Fact]
    public void HandlerProcessedMessages_NumberOfMessageIDsBelowMaxLimit_NumberOfProcessedMessaggesAndInfoString()
    {

    }
}