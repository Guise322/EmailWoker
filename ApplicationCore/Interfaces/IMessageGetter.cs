using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IMessageGetter
    {
        MimeMessage GetMessage(UniqueId id);
        MimeMessage GetMessage(int index);
    }
}