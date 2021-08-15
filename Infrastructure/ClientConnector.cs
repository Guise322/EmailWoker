using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure
{
    public class ClientConnector : IClientConnector
    {
        private ImapClient Client { get; }
        public ClientConnector(ImapClient client)
        {
            Client = client;
        }
        public void ConnectClient(EmailCredentials emailCredentials)
        {
            Client.Connect(emailCredentials.MailServer,
                emailCredentials.Port, emailCredentials.Ssl);
            Client.AuthenticationMechanisms.Remove("XOAUTH2");
            Client.Authenticate(emailCredentials.Login, emailCredentials.Password);
        }
        public void DisconnectClient()
        {
            Client.Disconnect(true);
        }
    }
}