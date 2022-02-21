using Xunit;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using System;
using System.Collections.Generic;
using MailKit;
using Moq;
using EmailWorker.Tests.UnitTests.Shared;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Tests.UnitTests.ApplicationCore;

public class MessageAnalyserUnitTest
{
    [Fact]
    public void AnalyzeMessages_Null_ThrowsArgumentNullException()
    {
        var ex = Record.Exception(() => MessageAnalyser.AnalyseMessages(null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIDsBelowMinLimit_ThrowsArgumentException()
    {
        int numberBelowMinLimit = 4;
        List<UniqueId> uniqueIDsShim = UniqueIDsShim.Create(numberBelowMinLimit);
        var ex = Record.Exception(() => MessageAnalyser.AnalyseMessages(uniqueIDsShim));
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIDsAboveMaxLimit_ListOfMaxLimitNumberWithMessageIDs()
    {
        int numberAboveMaxLimit = 1000;
        List<UniqueId> uniqueIDsShim = UniqueIDsShim.Create(numberAboveMaxLimit);
        var actual = MessageAnalyser.AnalyseMessages(uniqueIDsShim);
        Assert.Equal(uniqueIDsShim, actual);
    }

    [Fact]
    public void AnalyzeMessages_ListOfUniqueIdsBetweenLimits_ListWithSameUniqueIDs()
    {
        int appropriateNumberOfMessages = 6;
        List<UniqueId> uniqueIdsShim = UniqueIDsShim.Create(appropriateNumberOfMessages);
        var actual = MessageAnalyser.AnalyseMessages(uniqueIdsShim);
        Assert.Equal(uniqueIdsShim, actual);
    }
}