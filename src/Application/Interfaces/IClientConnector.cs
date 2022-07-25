namespace EmailWorker.Application.Interfaces;

public interface IClientConnector
{
    void ConnectClient(EmailCredentials emailCredentials);
    void DisconnectClient();
}