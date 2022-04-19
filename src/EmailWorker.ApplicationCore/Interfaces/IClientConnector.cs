using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.ApplicationCore.Interfaces;

public interface IClientConnector
{
    void ConnectClient(EmailCredentials emailCredentials);
    void DisconnectClient();
}