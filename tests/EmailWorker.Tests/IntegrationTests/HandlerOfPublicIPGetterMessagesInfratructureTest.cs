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
        Mock<IHttpClientFactory> factoryStub = new();

        factoryStub.Setup(p => p.CreateClient(It.IsAny<string>()))
            .Returns(() => new HttpClient());

        HandlerOfPublicIPGetterMessages handler = new(factoryStub.Object);
        (string actualEmailText, string actualEmailSubject) = handler.HandleProcessedMessages(null);
        Assert.Matches(@"\d", actualEmailText);
    }
}