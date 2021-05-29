using System;
using System.IO;
using System.Net;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using EmailWorker.Shared;

namespace EmailWorker.Models
{
    public class GetPublicIPByEmailModel : EmailWorkBase
    {
        public override bool ProcessResults(SearchResults results)
        {
            foreach (var uniqueId in results.UniqueIds)
            {
                MimeMessage message = _client.Inbox.GetMessage(uniqueId);
                string rawEmailFrom = message.From.ToString();
                
                string emailFrom = ExtractEmailFrom(rawEmailFrom);

                if (emailFrom == _myEmail)
                {
                    _client.Inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                    _client.Disconnect(true);
                    return true;
                }
                //Mark message as read
                _client.Inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
            }
            _client.Disconnect(true);
            return false;
        }

        private string _myIP;
        public override void SendAnswerBySmtp(MimeMessage message)
        {
            int smtpPort = 465;
            
            using (var client = new SmtpClient())
            {   
                _myIP = GetPublicIPAddress();

                client.Connect(_mailServer, smtpPort, _ssl);
                client.Authenticate(_login, _password);

                client.Send(message);

                client.Disconnect(true);
            }
        }
        public override MimeMessage BuildAnswerMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Worker", _login));
            message.To.Add(new MailboxAddress("Dmitry", _myEmail));
            message.Subject = "Ip By Email Project";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The current IP of the computer is {0}", _myIP)
            };
            return message;
        }
        private string ExtractEmailFrom(string rawString)
        {
            int first = rawString.IndexOf('<');
            int last = rawString.IndexOf('>');

            string emailFrom;

            if (first !> 0) 
            {
                emailFrom = rawString;
                return emailFrom;    
            }

            emailFrom = rawString.Substring(first + 1, last - first - 1);
            return emailFrom;
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
    }
}