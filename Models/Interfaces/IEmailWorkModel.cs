using System;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;
using MimeKit;
using System.Collections.Generic;

namespace EmailWorker.Models.Interfaces
{
    public interface IEmailWorkModel
    {
        IList<object> GetUnseenMessagesIDsFromInbox();
        IEmailWorkModel ProcessResults(IList<object> messagesIDs);
        IEmailWorkModel BuildAnswerMessage();
        void SendAnswerBySmtp();        
        void ProcessEmailbox();
    }   
}