using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IGetterOfUnseenMessageIDs
    {
        Task<IList<UniqueId>> GetUnseenMessageIDsAsync(EmailCredentials emailCredentials);
    }
}