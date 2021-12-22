using System;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Imap;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure
{
    public class ClientConnector : IClientConnector
    {
        private readonly ILogger<ClientConnector> _logger;
        private ImapClient Client { get; }
        public ClientConnector(ILogger<ClientConnector> logger, ImapClient client) =>
        (_logger, Client) = (logger, client);
        public void ConnectClient(EmailCredentials emailCredentials)
        {
            try
            {
                Client.Connect(emailCredentials.MailServer,
                emailCredentials.Port, emailCredentials.Ssl);
                Client.AuthenticationMechanisms.Remove("XOAUTH2");
                Client.Authenticate(emailCredentials.Login, emailCredentials.Password);

                _logger.LogInformation("The client is authenticated.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Connecting to the Imap Client is unsuccessful.");
                throw new Exception();
            }
        }
        public void DisconnectClient()
        {
            Client.Disconnect(true);
        }
    }
}