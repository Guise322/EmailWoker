using System;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;

public interface IEmailModel
{
    SearchResults GetUnseenMessagesFromInbox();
    void GetEmailCredentials(EmailCredentials credentials);
    bool ProcessResults(SearchResults results);
    void SendAnswerBySmtp();
    void BuildMessage();
}