using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;

namespace EmailWorker.Infrastructure
{
    public class AsSeenMarker : SeenFlagAdder, IAsSeenMarker
    {
        public AsSeenMarker(ImapClient client) : base(client) { }
        public EmailData MarkAsSeen(List<UniqueId> messages)
        {
            AddSeenFlag(messages);
            
            return new EmailData()
            {
                EmailSubject = "Mark As Seen Service",
                EmailText = "The count of messages marked as seen" +
                    messages.Count.ToString()
            };
        }
    }
}