using System;
using System.IO;
using System.Net;
using EmailWorker.Shared;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;

namespace EmailWorker.Models
{
    public class MarkAsSeen : EmailWorkBase
    {
        public MarkAsSeen(EmailCredentials emailCredentials) : base(emailCredentials)
        {

        }
        private int UnseenEmailsCount = 0;
        public override bool ProcessResults(SearchResults results)
        {
            int EmailsCount = 5;
            UnseenEmailsCount = results.Count;
            if (results.Count > EmailsCount)
            {
                for (int i = 0; i < EmailsCount; i++)
                {
                    _client.Inbox.AddFlags(results.UniqueIds[i], MessageFlags.Seen, true);   
                }
                _client.Disconnect(true);
                return true;
            }

            return false;
        }
        public override void SendAnswerBySmtp(MimeMessage message)
        {
            int smtpPort = 465;

            using (var client = new SmtpClient())
            {
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
            message.Subject = "The count of messages marked as seen";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The count of seen messages equals {0}", UnseenEmailsCount)
            };
            return message;
        }
    }
}