using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using EmailWorker.Shared;

namespace EmailWorker.Models
{
    public abstract class EmailWorkBase : IEmailModel
    {
        protected string _mailServer, _login, _password;
        protected int _port;
        protected bool _ssl;
        protected string _myEmail = "guise322@yandex.ru";
        protected ImapClient _client;
        public EmailWorkBase()
        {
            this._client = new ImapClient();
        }
        public void GetEmailCredentials(EmailCredentials credentials)
        {
            this._mailServer = credentials.MailServer;
            this._port = credentials.Port;
            this._ssl = credentials.Ssl;
            this._login = credentials.Login;
            this._password = credentials.Password;
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
        public abstract void SendAnswerBySmtp();
    }
}