using System;
using EmailWorker.Domain;
using MailKit;
using MimeKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.UnitTests.Application.PublicIPGetterServiceAggregate;

public class RequestMessageSearcherUnitTest
{
    [Fact]
    public void IsRequestMessage_WrongStringFormat_FormatException()
    {
        string rawEmailString = "testEmail@mail.com";
        string searchedEmail = "testEmail@mail.com";
        var actualException = Record.Exception(() => 
            RequestMessageSearcher.IsRequestMessage(rawEmailString, searchedEmail));
        Assert.IsType<FormatException>(actualException);
    }

    [Fact]
    public void IsRequestMessage_DistinctEmails_FormatException()
    {
        string rawEmailString = "<test1Email@mail.com>";
        string searchedEmail = "test2Email@mail.com";
        bool actual =  
            RequestMessageSearcher.IsRequestMessage(rawEmailString, searchedEmail);
        Assert.False(actual);
    }

    [Fact]
    public void IsRequestMessage_SameEmails_True()
    {
        string rawEmailString = "<testEmail@mail.com>";
        string searchedEmail = "testEmail@mail.com";
        bool actual =  
            RequestMessageSearcher.IsRequestMessage(rawEmailString, searchedEmail);
        Assert.True(actual);
    }
}