using System;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailWorker.Models.Interfaces
{
    public interface IEmailWorkModel
    {
        Task<IList<object>> GetUnseenMessagesIDsFromInboxAsync();
        IEmailWorkModel ProcessResults(IList<object> messagesIDs);
        IEmailWorkModel BuildAnswerMessage();
        void SendAnswerBySmtp();        
        Task ProcessEmailboxAsync();
    }   
}