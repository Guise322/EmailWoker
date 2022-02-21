
using System.Collections.Generic;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces;

public interface IRequestMessageSearcher
{
    List<UniqueId> SearchRequestMessage(IList<UniqueId> messageIDs, string searchedEmail);
}