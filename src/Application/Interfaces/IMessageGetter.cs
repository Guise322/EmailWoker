using MailKit;
using MimeKit;

namespace EmailWorker.Application.Interfaces;

public interface IMessageGetter
{
    MimeMessage GetMessage(UniqueId id);
}