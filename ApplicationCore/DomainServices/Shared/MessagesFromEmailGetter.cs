using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices.Shared
{
    public static class MessageIDsFromEmailGetter
    {
        public static Task<IList<UniqueId>> GetMessageIDsFromEmail(IClientConnector clientConnector,
            IGetterOfUnseenMessageIDs getterOfUnseenMessageIDs,
            EmailCredentials emailCredentials)
        {
            clientConnector.ConnectClient(emailCredentials);
            return getterOfUnseenMessageIDs.GetUnseenMessageIDsAsync(emailCredentials);
        }
    }
}