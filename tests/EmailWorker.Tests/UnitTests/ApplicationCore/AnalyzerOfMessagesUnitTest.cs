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
    public void AnalyzeMessages_Null_ThrowsArgumentNullException()
    {
        Mock<ILogger> loggerStub = new();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIDsBelowMinLimit_ThrowsArgumentException()
    {
        int numberBelowMinLimit = 4;
        List<UniqueId> uniqueIDsShim = UniqueIDsShim.Create(numberBelowMinLimit);
        Mock<ILogger> loggerStub = new();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, uniqueIDsShim));
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIDsAboveMaxLimit_ListOfMaxLimitNumberWithMessageIDs()
    {
        int numberAboveMaxLimit = 1000;
        List<UniqueId> uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        Mock<ILogger> loggerStub = new();
        var actual = AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, uniqueIDsShim);
        Assert.Equal(uniqueIDsShim, actual);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIdsBetweenLimits_ListWithSameUniqueIDs()
    {
        int appropriateNumberOfMessages = 6;
        List<UniqueId> uniqueIdsShim = UniqueIDsShim.Create(appropriateNumberOfMessages);
        Mock<ILogger> loggerStub = new();
        var actual = AnalyzerOfMessages.AnalyzeMessages(loggerStub.Object, uniqueIdsShim);
        Assert.Equal(uniqueIdsShim, actual);
    }
}