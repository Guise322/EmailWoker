using System;
using MailKit;
using System.Collections.Generic;
using MailKit.Net.Imap;
using System.Threading.Tasks;

namespace EmailWorker.Models.Interfaces
{
    public interface IEmailBoxWorkModel
    {
        Task<IList<object>> GetUnseenMessagesFromInboxAsync(ImapClient client);
    }
}