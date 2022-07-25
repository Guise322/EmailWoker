using MailKit;

namespace EmailWorker.Application.Interfaces;

public interface IUnseenMessageIdGetter
{
    Task<IList<UniqueId>> GetUnseenMessageIds();
}