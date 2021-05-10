using System;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

public interface IEmailModel
{
    SearchResults GetUnseenMessagesFromInbox();
    bool ProcessResults(SearchResults results);
    void SendAnswerBySmtp();
}