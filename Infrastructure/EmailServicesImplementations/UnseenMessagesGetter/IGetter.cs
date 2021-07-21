using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.UnseenMessagesGetter
{
    public interface IGetter
    {
        Task<List<object>> GetUnseenMessagesAsync();
    }
}