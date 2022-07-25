using MailKit;

namespace EmailWorker.Application.Interfaces;

public interface IAsSeenMarker
{
    EmailData MarkAsSeen(List<UniqueId> messages);
}