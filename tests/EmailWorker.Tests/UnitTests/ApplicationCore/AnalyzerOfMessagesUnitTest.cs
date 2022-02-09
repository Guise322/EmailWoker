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
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIDsBelowMinLimit_ThrowsArgumentException()
    {
        int numberBelowMinLimit = 4;
        List<UniqueId> uniqueIDsShim = UniqueIDsShim.Create(numberBelowMinLimit);
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(uniqueIDsShim));
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIDsAboveMaxLimit_ListOfMaxLimitNumberWithMessageIDs()
    {
        int numberAboveMaxLimit = 1000;
        List<UniqueId> uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        var actual = AnalyzerOfMessages.AnalyzeMessages(uniqueIDsShim);
        Assert.Equal(uniqueIDsShim, actual);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIdsBetweenLimits_ListWithSameUniqueIDs()
    {
        int appropriateNumberOfMessages = 6;
        List<UniqueId> uniqueIdsShim = UniqueIDsShim.Create(appropriateNumberOfMessages);
        var actual = AnalyzerOfMessages.AnalyzeMessages(uniqueIdsShim);
        Assert.Equal(uniqueIdsShim, actual);
    }
}