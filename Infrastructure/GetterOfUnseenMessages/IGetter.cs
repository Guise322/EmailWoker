using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailWorker.Infrastructure.EmailProcessor.GetterOfUnseenMessages
{
    public interface IGetter
    {
        Task<List<object>> GetUnseenMessagesAsync();
    }
}