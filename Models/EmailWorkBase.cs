using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;
using MimeKit;
using System.Collections.Generic;
using EmailWorker.Models.Interfaces;
using System.Threading.Tasks;

namespace EmailWorker.Models
{
    public abstract class EmailWorkBase : IEmailWorkModel
    {
        protected MimeMessage Message;
        protected bool RequestIsGot = false;
        private IEmailBoxWorkModel emailBoxWork;
        protected readonly string MyEmail = "guise322@yandex.ru";
        protected readonly EmailCredentials EmailCredentials;
        protected readonly ImapClient Client;
        public EmailWorkBase(EmailCredentials emailCredentials, IEmailBoxWorkModel emailBoxWork)
        {
            Client = new ImapClient();
            EmailCredentials = emailCredentials;
            this.emailBoxWork = emailBoxWork;
        }
        public Task<IList<object>> GetUnseenMessagesIDsFromInboxAsync() =>
            emailBoxWork.GetUnseenMessagesFromInboxAsync(Client);
        public abstract IEmailWorkModel ProcessResults(IList<object> messagesIDs);
        public abstract void SendAnswerBySmtp();
        public abstract IEmailWorkModel BuildAnswerMessage();
        public async Task ProcessEmailboxAsync()
        {
            IList<object> messagesIDs = await GetUnseenMessagesIDsFromInboxAsync();
            ProcessResults(messagesIDs).BuildAnswerMessage().SendAnswerBySmtp();
        }
    }
}