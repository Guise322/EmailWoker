using Xunit;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using System;
using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.ApplicationCore;

public class AnalyzerOfMessagesUnitTest
{
    [Fact]
    public void AnalyzeMessages_MessagesUnderMinThreshold_ThrowsArgumentException()
    {
        var loggerStub = new LoggerStub();
        int valueUnderMinThreshold = 4;
        List<UniqueId> uniqueIdsStub = new(valueUnderMinThreshold);

        for (uint i = 0; i < (uint)valueUnderMinThreshold; i++)
        {
            uniqueIdsStub.Add(new UniqueId(i+1));
        }

        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub, uniqueIdsStub));
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void AnalyzeMessages_NullInput_ThrowsArgumentNullException()
    {
        var loggerStub = new LoggerStub();
        var ex = Record.Exception(() => AnalyzerOfMessages.AnalyzeMessages(loggerStub, null));
        Assert.IsType<ArgumentNullException>(ex);
    }
}