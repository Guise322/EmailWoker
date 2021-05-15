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
using EmailWorker.Shared;

namespace EmailWorker.Models
{
    public class GetPublicIPByEmailModel : IEmailModel
    {
        private ImapClient _client;

        private string _mailServer, _login, _password;
        private int _port;
        private bool _ssl;
        private string _myEmail = "guise322@yandex.ru";
        public GetPublicIPByEmailModel()
        {
            this._client = new ImapClient();
        }

        public void GetEmailCredentials(EmailCredentials credentials)
        {
            this._mailServer = credentials.MailServer;
            this._port = credentials.Port;
            this._ssl = credentials.Ssl;
            this._login = credentials.Login;
            this._password = credentials.Password;
        }

        public SearchResults GetUnseenMessagesFromInbox()
        {
            _client.Connect(_mailServer, _port, _ssl);
            _client.AuthenticationMechanisms.Remove("XOAUTH2");
            _client.Authenticate(_login, _password);
            _client.Inbox.Open(FolderAccess.ReadWrite);
            return _client.Inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
        }

        public bool ProcessResults(SearchResults results)
        {
            foreach (var uniqueId in results.UniqueIds)
            {
                MimeMessage message = _client.Inbox.GetMessage(uniqueId);
                string rawEmailFrom = message.From.ToString();
                int start = rawEmailFrom.IndexOf('<');
                int end = rawEmailFrom.IndexOf('>');

                string emailFrom;

                if (start > 0) emailFrom = rawEmailFrom.Substring(start + 1, end - start - 1);
                else emailFrom = rawEmailFrom;

                if (emailFrom == _myEmail)
                {
                    _client.Inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                    return true;
                    //SendAnswerBySmtp();
                }
                else
                {
                    //Mark message as read
                    _client.Inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }
            }
            _client.Disconnect(true);
            
            return false;
        }

        public void SendAnswerBySmtp()
        {
            int smtpPort = 465;

            using (var client = new SmtpClient())
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    string myIP = GetPublicIPAddress();

                    client.Connect(_mailServer, smtpPort, _ssl);
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