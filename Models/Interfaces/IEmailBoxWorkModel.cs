using System;
using MailKit;
using System.Collections.Generic;
using MailKit.Net.Imap;

namespace EmailWorker.Models.Interfaces
{
    public interface IEmailBoxWorkModel
    {
        IList<object> GetUnseenMessagesFromInbox(ImapClient client);
    }
}