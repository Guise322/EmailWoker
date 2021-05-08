using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;

namespace IpByEmail.MailRepository
{
public class MailRepositoryClass// : IMailRepository
{
    private readonly string _mailServer, _login, _password;
    private readonly int _port;
    private readonly bool _ssl;

    private readonly string _myEmail = "guise322@yandex.ru";
    public MailRepositoryClass(string mailServer, int port, bool ssl, string login, string password)
    {
        this._mailServer = mailServer;
        this._port = port;
        this._ssl = ssl;
        this._login = login;
        this._password = password;
    }

    public IEnumerable<string> GetUnreadMails()
    {
        var subjects = new List<string>();

        using (var client = new ImapClient())
        {
            client.Connect(_mailServer, _port, _ssl);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(_login, _password);

            // The Inbox folder is always available on all IMAP servers...
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadWrite);
            var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
            
            foreach(var uniqueId in results.UniqueIds)
            {
                MimeMessage message = inbox.GetMessage(uniqueId);
                string rawEmailFrom = message.From.ToString();
                int start = rawEmailFrom.IndexOf('<');

                if (start > 0)
                    rawEmailFrom = rawEmailFrom.Remove(0, start + 1);

                int end = rawEmailFrom.IndexOf('>');

                string emailFrom;
                if (end > 0)
                    emailFrom = rawEmailFrom.Remove(end);
                else
                    emailFrom = rawEmailFrom;
                    
                emailFrom = emailFrom.ToLower();

                if (emailFrom == _myEmail)
                {
                    subjects.Add(message.Subject);
                }
                else
                {
                    //Mark message as read
                    inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }
            }

            client.Disconnect(true);
        }
        return subjects;
    }

    private void SendMessageBySmtp()
    {
     int smtpPort = 465;

     using (var client = new SmtpClient())
     {
         
     }   
    }

    public IEnumerable<string> GetAllMails()
    {
        var messages = new List<string>();

        using (var client = new ImapClient())
        {
            client.Connect(_mailServer, _port, _ssl);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(_login, _password);

            // The Inbox folder is always available on all IMAP servers...
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            var results = inbox.Search(SearchOptions.All, SearchQuery.NotSeen);
            foreach (var uniqueId in results.UniqueIds)
            {
                var message = inbox.GetMessage(uniqueId);

                //Mark message as read
                //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
            }

            client.Disconnect(true);
        }

        return messages;
    }
}
}