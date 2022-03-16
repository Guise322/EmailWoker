using System.Collections.Generic;
using Ardalis.GuardClauses;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure;

public class SeenFlagAdder
{
    protected IImapClient Client;
    public SeenFlagAdder(IImapClient client) => Client = client;
    protected void AddSeenFlag(List<UniqueId> messages)
    {
        Guard.Against.Null(messages, nameof(messages));
        
        foreach (var message in messages)
        {
            Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
        }
    }
}