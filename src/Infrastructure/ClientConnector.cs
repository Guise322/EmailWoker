using EmailWorker.Application;
using EmailWorker.Application.Interfaces;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure;

public class ClientConnector : IClientConnector
{
    private readonly IImapClient _client;
    public ClientConnector(IImapClient client)
    {
        _client = client;
    }

    public void ConnectClient(EmailCredentials emailCredentials)
    {
        _client.Connect(emailCredentials.MailServer,
        emailCredentials.Port, emailCredentials.Ssl);
        _client.AuthenticationMechanisms.Remove("XOAUTH2");
        _client.Authenticate(emailCredentials.Login, emailCredentials.Password);
    }

    public void DisconnectClient()
    {
        _client.Disconnect(true);
        _client.Dispose();
    }
}