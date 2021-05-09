using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
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
                int end = rawEmailFrom.IndexOf('>');

                string emailFrom;
                
                if (start > 0) emailFrom = rawEmailFrom.Substring(start + 1, end - start - 1);
                else emailFrom = rawEmailFrom;

                if (emailFrom == _myEmail)
                {
                    SendAnswerBySmtp();
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

    private void SendAnswerBySmtp()
    {
     int smtpPort = 465;

     using (var client = new SmtpClient())
     {
        if (NetworkInterface.GetIsNetworkAvailable())
        {
            string myIP = GetPublicIPAddress();

            client.Connect(_mailServer,smtpPort, _ssl);
            client.Authenticate(_login, _password);

            var answerMessage = new MimeMessage();
            answerMessage.From.Add(new MailboxAddress("Worker", _login));
            answerMessage.To.Add(new MailboxAddress("Dmitry", _myEmail));
            answerMessage.Subject = "Ip By Email Project";
            answerMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The current IP of the computer is {0}", myIP)
            };
        
            client.Send(answerMessage);

            client.Disconnect(true);
        }
        else throw new Exception("No connection to the Internet!");
        
        }   
    }

    private string GetPublicIPAddress()  
    {  
        string address;  
        WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");  
        using (WebResponse response = request.GetResponse())  
        using (StreamReader stream = new StreamReader(response.GetResponseStream()))  
        {  
            address = stream.ReadToEnd();  
        }  
  
        int first = address.IndexOf("Address: ") + 9;  
        int last = address.LastIndexOf("</body>");  
        address = address.Substring(first, last - first);  
  
        return address;  
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