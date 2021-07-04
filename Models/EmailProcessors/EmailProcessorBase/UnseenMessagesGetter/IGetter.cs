using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase.UnseenMessagesGetter
{
    public interface IGetter
    {
        Task<IList<object>> GetUnseenMessagesAsync(IEmailProcessorModel model);
    }
}