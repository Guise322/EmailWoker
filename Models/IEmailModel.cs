using System;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using IpByEmail.Shared;

public interface IEmailModel
{
    SearchResults GetUnseenMessagesFromInbox();
    void GetEmailCredentials(EmailCredentials credentials);
    bool ProcessResults(SearchResults results);
    void SendAnswerBySmtp();
}