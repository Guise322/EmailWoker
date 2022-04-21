using Xunit;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using System;
using System.Collections.Generic;
using MailKit;
using Moq;
using EmailWorker.Tests.UnitTests.Shared;

namespace EmailWorker.Tests.UnitTests.ApplicationCore;

public class MessageAnalyserUnitTest
{
    [Fact]
    public void AnalyzeMessages_Null_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => MessageAnalyser.AnalyseMessages(null));
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void AnalyzeMessages_UniqueIDListBelowMinLimit_False()
    {
        int numberBelowMinLimit = 4;
        List<UniqueId> uniqueIDsShim = UniqueIDListShim.Create(numberBelowMinLimit);
        bool actual = MessageAnalyser.AnalyseMessages(uniqueIDsShim);
        Assert.False(actual);
    }

    [Fact]
    public void AnalyzeMessages_UniqueIdListAboveMinLimit_True()
    {
        int appropriateNumberOfMessages = 6;
        List<UniqueId> uniqueIdsShim = UniqueIDListShim.Create(appropriateNumberOfMessages);
        bool actual = MessageAnalyser.AnalyseMessages(uniqueIdsShim);
        Assert.True(actual);
    }
}