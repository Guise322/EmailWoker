using System;
using System.IO;
using System.Net;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using EmailWorker.Shared;
using System.Collections.Generic;
using EmailWorker.Models.Interfaces;

namespace EmailWorker.Models
{
    public class GetPublicIPByEmailModel : EmailWorkBase
    {
        public GetPublicIPByEmailModel(EmailCredentials emailCredentials,
            IEmailBoxWorkModel emailBoxWork) : base(emailCredentials, emailBoxWork)
        {

        }
        public override IEmailWorkModel ProcessResults(IList<object> messagesIDs)
        {
            bool isUniqueId = messagesIDs is IList<UniqueId>;

            foreach (var item in messagesIDs)
            {
                MimeMessage message = isUniqueId ?
                    Client.Inbox.GetMessage((UniqueId)item) : Client.Inbox.GetMessage((int)item);
                string rawEmailFrom = message.From.ToString();
                
                string emailFrom = ExtractEmailFrom(rawEmailFrom);

                if (emailFrom == MyEmail)
                {
                    RequestIsGot = true;
                    return this;
                }
                if (isUniqueId)
                {
                    Client.Inbox.AddFlags((UniqueId)item, MessageFlags.Seen, true);
                }
                else
                {
                    Client.Inbox.AddFlags((int)item, MessageFlags.Seen, true);
                }
            }
            Client.Disconnect(true);
            return this;
        }

        private string myIP;
        public override void SendAnswerBySmtp()
        {
            int smtpPort = 465;
            
            using (var client = new SmtpClient())
            {   
                myIP = GetPublicIPAddress();

                client.Connect(EmailCredentials.MailServer, smtpPort, EmailCredentials.Ssl);
                client.Authenticate(EmailCredentials.Login, EmailCredentials.Password);

                client.Send(Message);

                client.Disconnect(true);
            }
        }
        public override IEmailWorkModel BuildAnswerMessage()
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("Worker", EmailCredentials.Login));
            message.To.Add(new MailboxAddress("Dmitry", MyEmail));
            message.Subject = "Ip By Email Project";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The current IP of the computer is {0}", myIP)
            };
            Message = message;
            return this;
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