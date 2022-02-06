using System;
using Xunit;
using EmailWorker.Infrastructure.HandlersOfProcessedMessages;
using Moq;
using Serilog;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Tests.IntegrationTests;

public class HandlerOfPublicIPGetterMessagesIntegrationTest
{
    [Fact]
    public void HandleProcessedMessages_HttpClientFactory_IPAddressAndInformationMessage()
    {
        Mock<ILogger<HandlerOfPublicIPGetterMessages>> loggerStub = new();
        Mock<IHttpClientFactory> factoryShim = new();

        factoryShim.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Returns(() => new());

        HandlerOfPublicIPGetterMessages handler = new(loggerStub.Object, factoryShim.Object);
        (string actualEmailText, string actualEmailSubject) = handler.HandleProcessedMessages(null);
        Assert.Matches(@"\d", actualEmailText);
    }
}