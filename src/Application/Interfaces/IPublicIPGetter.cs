using MailKit;

namespace EmailWorker.Application.Interfaces;

public interface IPublicIPGetter
{
    EmailData GetPublicIP();
}