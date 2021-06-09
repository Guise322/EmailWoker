using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using EmailWorker.Models.Interfaces;
using EmailWorker.Shared;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;

namespace EmailWorker.Models
{
    public class MarkAsSeen : EmailWorkBase
    {
        public MarkAsSeen(EmailCredentials emailCredentials,
            IEmailBoxWorkModel emailBoxWork) : base(emailCredentials, emailBoxWork)
        {

        }
        private int UnseenEmailsCount = 0;
        public override IEmailWorkModel ProcessResults(IList<object> messagesIDs)
        {
            int EmailsCount = 5;
            bool isUniqueId = messagesIDs is IList<UniqueId>;

            UnseenEmailsCount = messagesIDs.Count;
            if (messagesIDs.Count > EmailsCount)
            {
                if (isUniqueId)
                {
                    for (int i = 0; i < EmailsCount; i++)
                    {
                        Client.Inbox.AddFlags((UniqueId)messagesIDs[i], MessageFlags.Seen, true);   
                    }
                }
                else
                {
                    for (int i = 0; i < EmailsCount; i++)
                    {
                        Client.Inbox.AddFlags((int)messagesIDs[i], MessageFlags.Seen, true);   
                    }
                }
                Client.Disconnect(true);
            }
            return this;
        }
        public override void SendAnswerBySmtp()
        {
            int smtpPort = 465;

            using (var client = new SmtpClient())
            {
                client.Connect(EmailCredentials.MailServer, smtpPort, EmailCredentials.Ssl);
                client.Authenticate(EmailCredentials.Login, EmailCredentials.Password);

                client.Send(Message);

                client.Disconnect(true);
            }
        }
        public override IEmailWorkModel BuildAnswerMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Worker", EmailCredentials.Login));
            message.To.Add(new MailboxAddress("Dmitry", MyEmail));
            message.Subject = "The count of messages marked as seen";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The count of seen messages equals {0}", UnseenEmailsCount)
            };
            Message = message;
            return this;
        }
    }
}