using MailKit;

namespace EmailWorker.Application.Interfaces;

internal interface IGetPublicIPProvider
{
    EmailData GetPublicIPProvider(List<UniqueId> messages);
}