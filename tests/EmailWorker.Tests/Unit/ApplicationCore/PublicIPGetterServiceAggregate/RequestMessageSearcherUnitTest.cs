using System;
using System.Collections.Generic;
using EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.Tests.Unit.Shared;
using MailKit;
using MimeKit;
using Moq;
using Xunit;

namespace EmailWorker.Tests.UnitTests.ApplicationCore.PublicIPGetterServiceAggregate;

public class RequestMessageSearcherUnitTest
{
    [Fact]
    public void SearchRequestMessage_EmptySearchedEmail_ArgumentException()
    {
        InternetAddressList fromInternetAddressListShim = new()
        { new MailboxAddress("TestFromName", "TestFromEmail") };
        InternetAddressList toInternetAddressListShim = new()
        { new MailboxAddress("TestToName", "TestToEmail") };
        MimeMessage mimeMessageShim = new(
            fromInternetAddressListShim,
            toInternetAddressListShim,
            "TestSubject",
            null
        );
        
        Mock<IMessageGetter> messageGetterStub = new();
        messageGetterStub.Setup(p => p.GetMessage(It.IsAny<UniqueId>()))
            .Returns(mimeMessageShim);

        List<UniqueId> uniqueIDShim = UniqueIDList.Create(1);

        RequestMessageSearcher requestMessageSearcher =
            new(messageGetterStub.Object);
        string testSearchedEmail = "";
        var actualException = Record.Exception(() => requestMessageSearcher
            .SearchRequestMessage(uniqueIDShim, testSearchedEmail));

        Assert.IsType<ArgumentException>(actualException);
    }

    [Fact]
    public void SearchRequestMessage_NullSearchedEmail_ArgumentNullException()
    {
        InternetAddressList fromInternetAddressListShim = new()
        { new MailboxAddress("TestFromName", "TestFromEmail") };
        InternetAddressList toInternetAddressListShim = new()
        { new MailboxAddress("TestToName", "TestToEmail") };
        MimeMessage mimeMessageShim = new(
            fromInternetAddressListShim,
            toInternetAddressListShim,
            "TestSubject",
            null
        );
        
        Mock<IMessageGetter> messageGetterStub = new();
        messageGetterStub.Setup(p => p.GetMessage(It.IsAny<UniqueId>()))
            .Returns(mimeMessageShim);

        List<UniqueId> uniqueIDShim = UniqueIDList.Create(1);

        RequestMessageSearcher requestMessageSearcher =
            new(messageGetterStub.Object);

        var actualException = Record.Exception(() => requestMessageSearcher
            .SearchRequestMessage(uniqueIDShim, null));

        Assert.IsType<ArgumentNullException>(actualException);
    }

    [Fact]
    public void SearchedRequestMessage_EmailAndIDListWithSearchedEmail_IDListOfMessagesContainingSearchedEmail()
    {
        InternetAddressList fromInternetAddressListShim = new()
        { new MailboxAddress("TestFromName", "TestFromEmail") };
        InternetAddressList toInternetAddressListShim = new()
        { new MailboxAddress("TestToName", "TestToEmail") };
        MimeMessage mimeMessageShim = new(
            fromInternetAddressListShim,
            toInternetAddressListShim,
            "TestSubject",
            null
        );
        
        Mock<IMessageGetter> messageGetterStub = new();
        messageGetterStub.Setup(p => p.GetMessage(It.IsAny<UniqueId>()))
            .Returns(mimeMessageShim);

        List<UniqueId> uniqueIDShim = UniqueIDList.Create(1);

        RequestMessageSearcher requestMessageSearcher =
            new(messageGetterStub.Object);
        string testSearchedEmail = "TestFromEmail";
        List<UniqueId> messageIDs = requestMessageSearcher
            .SearchRequestMessage(uniqueIDShim, testSearchedEmail);

        Assert.Equal(uniqueIDShim[0], messageIDs[0]);
    }

    [Fact]
    public void SearchRequestMessage_EmailAndIDListWithDistinctEmails_EmptyIDList()
    {
        InternetAddressList fromInternetAddressListShim = new()
        { new MailboxAddress("TestDistinctFromName", "TestDistinctFromEmail") };
        InternetAddressList toInternetAddressListShim = new()
        { new MailboxAddress("TestToName", "TestToEmail") };
        MimeMessage mimeMessageShim = new(
            fromInternetAddressListShim,
            toInternetAddressListShim,
            "TestSubject",
            null
        );
        
        Mock<IMessageGetter> messageGetterStub = new();
        messageGetterStub.Setup(p => p.GetMessage(It.IsAny<UniqueId>()))
            .Returns(mimeMessageShim);

        List<UniqueId> uniqueIDShim = UniqueIDList.Create(1);

        RequestMessageSearcher requestMessageSearcher =
            new(messageGetterStub.Object);
        string testSearchedEmail = "TestFromEmail";
        List<UniqueId> messageIDs = requestMessageSearcher
            .SearchRequestMessage(uniqueIDShim, testSearchedEmail);

        Assert.Empty(messageIDs);
    }
}