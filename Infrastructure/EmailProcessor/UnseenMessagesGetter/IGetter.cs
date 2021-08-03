using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailWorker.Infrastructure.EmailProcessor.UnseenMessagesGetter
{
    public interface IGetter
    {
        Task<List<object>> GetUnseenMessagesAsync();
    }
}