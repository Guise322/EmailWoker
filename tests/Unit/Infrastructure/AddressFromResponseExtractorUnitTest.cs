using System;
using EmailWorker.Infrastructure;
using Xunit;

namespace EmailWorker.Tests.Unit.Infrastructure;

public class AddressFromResponseExtractorUnitTest
{
    [Fact]
    public void AddressFromResponseExtractor_EmptyResponseString_ArgumentException()
    {
        string testResponse = "";
        var actualException = Record.Exception(() => AddressFromResponseExtractor
            .ExtractAddressFromResponse(testResponse));
        Assert.IsType<ArgumentException>(actualException);
    }

    [Fact]
    public void AddressFromResponseExtractor_ResponseStringInInvalidFormat_FormatException()
    {
        string testResponse = "<body>someText</body>";
        var actualException = Record.Exception(() => AddressFromResponseExtractor
            .ExtractAddressFromResponse(testResponse));
        Assert.IsType<FormatException>(actualException);
    }

    [Fact]
    public void AddressFromResponseExtractor_ResponseString_AppropriateString()
    {
        string testResponse = "<body>Address: 111.111.111.111</body>";
        string actualAddress = AddressFromResponseExtractor
            .ExtractAddressFromResponse(testResponse);
        string expectedAddress = "111.111.111.111";
        Assert.Equal(expectedAddress, actualAddress);
    }
}