using Microsoft.Extensions.Logging;
using System;

namespace EmailWorker.Tests.UnitTests.ApplicationCore;

public class LoggerStub : ILogger
{
    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter
        )
    {
        
    }
}