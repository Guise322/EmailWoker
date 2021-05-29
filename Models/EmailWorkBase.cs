using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;
using MimeKit;

namespace EmailWorker.Models
{
    public abstract class EmailWorkBase : IEmailModel
    {
        protected readonly string _myEmail = "guise322@yandex.ru";
        protected string _mailServer, _login, _password;
        protected int _port;
        protected bool _ssl;
        protected ImapClient _client;
        public EmailWorkBase()
        {
            _client = new ImapClient();
        }
        public void SeedEmailCredentials(EmailCredentials credentialsToken)
        {
            _mailServer = credentialsToken.MailServer;
            _port = credentialsToken.Port;
            _ssl = credentialsToken.Ssl;
            _login = credentialsToken.Login;
            _password = credentialsToken.Password;
        }
        public SearchResults GetUnseenMessagesFromInbox()
        {
            _client.Connect(_mailServer, _port, _ssl);
            _client.AuthenticationMechanisms.Remove("XOAUTH2");
            _client.Authenticate(_login, _password);
            _client.Inbox.Open(FolderAccess.ReadWrite);
            return _client.Inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
        }
        public abstract bool ProcessResults(SearchResults results);
        public abstract void SendAnswerBySmtp(MimeMessage message);
        public abstract MimeMessage BuildAnswerMessage();
        public void DoProcess()
        {
            SearchResults results = GetUnseenMessagesFromInbox();
            if (ProcessResults(results))
            {
                MimeMessage message = BuildAnswerMessage();
                SendAnswerBySmtp(message);
            }
        }
    }
}