using System.Collections.Generic;
using Ardalis.GuardClauses;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure;

public class SeenFlagAdder
{
    protected ImapClient Client;
    public SeenFlagAdder(ImapClient client) => Client = client;
    protected void AddSeenFlag(List<UniqueId> messages)
    {
        Guard.Against.Null(messages, nameof(messages));
        
        foreach (var message in messages)
        {
            Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
        }
    }
}