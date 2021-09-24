using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices.Shared
{
    public static class MessagesFromEmailGetter
    {
        public static Task<IList<UniqueId>> GetMessagesFromEmail(IClientConnector clientConnector,
            IGetterOfUnseenMessages getterOfUnseenMessages,
            EmailCredentials emailCredentials)
        {
            clientConnector.ConnectClient(emailCredentials);
            return getterOfUnseenMessages.GetUnseenMessagesAsync(emailCredentials);
        }
    }
}