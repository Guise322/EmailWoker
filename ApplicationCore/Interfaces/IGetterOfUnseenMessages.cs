using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IGetterOfUnseenMessages
    {
        Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials);
    }
}