using System;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using Moq;
using Xunit;

namespace EmailWorker.Tests.Unit.ApplicationCore.PublicIPGetterServiceAggregate;

public class EmailExtractorUnitTest
{
    [Fact]
    public void ExtractEmail_WrongStringFormat_WrongFormatException()
    {
        string emailString = "testEmail@mail.com";
        var actualException = Record.Exception(() => 
            EmailExtractor.ExtractEmail(emailString));
        Assert.IsType<FormatException>(actualException);

    }

    [Fact]
    public void ExtractEmail_EmailStringInMimeMessageFromFormat_EmailString()
    {
        string emailString = "<testEmail@mail.com>";
        string actualEmail = EmailExtractor.ExtractEmail(emailString);
        string expectedEmail = "testEmail@mail.com";
        Assert.Equal(expectedEmail, actualEmail);
    }
}