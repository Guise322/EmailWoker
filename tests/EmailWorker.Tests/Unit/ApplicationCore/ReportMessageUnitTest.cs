using System;
using EmailWorker.ApplicationCore.DomainServices;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.Unit.ApplicationCore;

public class ReportMessageUnitTest
{
    [Fact]
    public void CreateReportMessage_NullStrings_ArgumentNullException()
    {
        string testLogin = "testLogin";
        string testEmailAddress = "testEmailAddress";
        EmailData testEmailData = new()
        { EmailSubject = "testEmailSubject", EmailText = "testEmailText" };
        
        var actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(null, testEmailAddress, testEmailData));
        Assert.IsType<ArgumentNullException>(actualException);

        actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(testLogin, null, testEmailData));
        Assert.IsType<ArgumentNullException>(actualException);

        testEmailData.EmailSubject = null;
        actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(testLogin, testEmailAddress, testEmailData));
        Assert.IsType<ArgumentNullException>(actualException);

        testEmailData.EmailSubject = "testEmailSubject";
        testEmailData.EmailText = null;
        actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(testLogin, testEmailAddress, testEmailData));
        Assert.IsType<ArgumentNullException>(actualException);
    }

    [Fact]
    public void CreateReportMessage_EmptyStrings_ArgumentException()
    {
        string emptyString = "";
        string testLogin = "testLogin";
        string testEmailAddress = "testEmailAddress";
        EmailData testEmailData = new()
        { EmailSubject = "testEmailSubject", EmailText = "testEmailText" };
        
        var actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(emptyString, testEmailAddress, testEmailData));
        Assert.IsType<ArgumentException>(actualException);

        actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(testLogin, emptyString, testEmailData));
        Assert.IsType<ArgumentException>(actualException);

        testEmailData.EmailSubject = emptyString;
        actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(testLogin, testEmailAddress, testEmailData));
        Assert.IsType<ArgumentException>(actualException);

        testEmailData.EmailSubject = "testEmailSubject";
        testEmailData.EmailText = emptyString;
        actualException = Record.Exception(() =>
            ReportMessage.CreateReportMessage(testLogin, testEmailAddress, testEmailData));
        Assert.IsType<ArgumentException>(actualException);
    }

    [Fact]
    public void CreateReportMessage_LoginAndEmailAddressStrings_MimeMessageWithAppropriatePropeties()
    {
        string testLogin = "testLogin";
        string testEmailAddress = "testEmailAddress";
        EmailData testEmailData = new()
        { EmailSubject = "testEmailSubject", EmailText = "testEmailText" };
        
        MimeMessage message =
            ReportMessage.CreateReportMessage(testLogin, testEmailAddress, testEmailData);
        Assert.Equal(testLogin, EmailExtractor.ExtractEmail(message.From.ToString()));
        Assert.Equal(testEmailAddress, EmailExtractor.ExtractEmail(message.To.ToString()));
        Assert.Equal(testEmailData.EmailSubject, message.Subject);
        Assert.Equal(testEmailData.EmailText, message.GetTextBody(MimeKit.Text.TextFormat.Plain));
    }
}