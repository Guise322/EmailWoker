using EmailWorker.ApplicationCore.Entities;
using MailKit.Net.Imap;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IClientConnector
    {
        void ConnectClient(EmailCredentials emailCredentials);
        void DisconnectClient();
    }
}