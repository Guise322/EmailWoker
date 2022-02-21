using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IAsSeenMarker
    {
        EmailData MarkAsSeen(List<UniqueId> messages);
    }
}