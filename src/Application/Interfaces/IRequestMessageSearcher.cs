using MailKit;

namespace EmailWorker.Application.Interfaces;

internal interface IRequestMessageSearcher
{
    List<UniqueId> SearchRequestMessage(IList<UniqueId> messageIDs, string searchedEmail);
}