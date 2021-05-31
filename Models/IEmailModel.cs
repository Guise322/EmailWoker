using System;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;
using MimeKit;

public interface IEmailModel
{
    SearchResults GetUnseenMessagesFromInbox();
    bool ProcessResults(SearchResults results);
    void SendAnswerBySmtp(MimeMessage message);
    MimeMessage BuildAnswerMessage();
    void ProcessEmailbox();
}