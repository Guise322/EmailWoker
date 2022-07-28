using System;
using EmailWorker.Infrastructure;
using Xunit;

namespace EmailWorker.Tests.Unit.Infrastructure;

public class AddressFromResponseExtractorTests
{
    [Fact]
    public void ExtractAddressFromResponse_EmptyResponseString_ArgumentException()
    {
        string testResponse = "";
        var actualException = Record.Exception(() => AddressFromResponseExtractor
            .ExtractAddressFromResponse(testResponse));
        Assert.IsType<FormatException>(actualException);
    }

    [Fact]
    public void ExtractAddressFromResponse_ResponseStringInInvalidFormat_FormatException()
    {
        string testResponse = "<body>someText</body>";
        var actualException = Record.Exception(() => AddressFromResponseExtractor
            .ExtractAddressFromResponse(testResponse));
        Assert.IsType<FormatException>(actualException);
    }

    [Fact]
    public void ExtractAddressFromResponse_ResponseString_AppropriateString()
    {
        string testResponse = "<body>Address: 111.111.111.111</body>";
        string actualAddress = AddressFromResponseExtractor
            .ExtractAddressFromResponse(testResponse);
        string expectedAddress = "111.111.111.111";
        Assert.Equal(expectedAddress, actualAddress);
    }
}