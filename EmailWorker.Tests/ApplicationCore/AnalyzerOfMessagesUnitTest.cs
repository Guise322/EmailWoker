using Xunit;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using System;
using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.ApplicationCore;

public class AnalyzerOfMessagesUnitTest
{
    [Fact]
    public void AnalyzeMessages_UniqueIDsUnderMinThreshold_ThrowsArgumentException()
    {
        int valueUnderMinThreshold = 4;
        List<UniqueId> uniqueIdsShim = CreateUniqueIdsShim(valueUnderMinThreshold);
        LoggerStub loggerStub = new();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub, uniqueIdsShim));
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_NullInput_ThrowsArgumentNullException()
    {
        LoggerStub loggerStub = new();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub, null));
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_AppropriateNumberOfUniqueIds_SameUniqueIDs()
    {
        int appropriateNumberOfMessages = 6;
        List<UniqueId> uniqueIdsShim = CreateUniqueIdsShim(appropriateNumberOfMessages);
        LoggerStub loggerStub = new();
        var actual = AnalyzerOfMessages.AnalyzeMessages(loggerStub, uniqueIdsShim);
        Assert.Equal(appropriateNumberOfMessages, actual.Count);
    }

    public static List<UniqueId> CreateUniqueIdsShim(int numberOfItems)
    {
        List<UniqueId> uniqueIdsShim = new(numberOfItems);

        for (uint i = 0; i < numberOfItems; i++)
        {
            uniqueIdsShim.Add(new UniqueId(i+1));
        }

        return uniqueIdsShim;
    }
}