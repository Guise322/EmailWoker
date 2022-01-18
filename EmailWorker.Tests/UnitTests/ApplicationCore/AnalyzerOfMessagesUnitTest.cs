using Xunit;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using System;
using System.Collections.Generic;
using MailKit;
using Moq;
using EmailWorker.Tests.UnitTests.Shared;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Tests.UnitTests.ApplicationCore;

public class AnalyzerOfMessagesUnitTest
{
    [Fact]
    public void AnalyzeMessages_UniqueIDsUnderMinThreshold_ThrowsArgumentException()
    {
        int valueUnderMinThreshold = 4;
        List<UniqueId> uniqueIdsShim = UniqueIdsShim.Create(valueUnderMinThreshold);
        Mock<ILogger> loggerStub = new();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, uniqueIdsShim));
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_NullInput_ThrowsArgumentNullException()
    {
        Mock<ILogger> loggerStub = new();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_AppropriateNumberOfUniqueIds_SameUniqueIDs()
    {
        int appropriateNumberOfMessages = 6;
        List<UniqueId> uniqueIdsShim = UniqueIdsShim.Create(appropriateNumberOfMessages);
        Mock<ILogger> loggerStub = new();
        var actual = AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, uniqueIdsShim);
        Assert.Equal(appropriateNumberOfMessages, actual.Count);
    }
}