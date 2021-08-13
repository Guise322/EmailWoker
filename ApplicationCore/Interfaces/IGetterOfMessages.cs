using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IMessageGetter
    {
        MimeMessage GetMessage(object id);
    }
}