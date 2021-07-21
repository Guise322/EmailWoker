using EmailWorker.ApplicationCore.Entities;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.UnseenMessagesGetter
{
    public class GetterBuilder
    {
        public static IGetter BuildGetter(EmailCredentials emailCredentials, ImapClient client)
        {
            if (emailCredentials.Login.Contains("ya") ||
                emailCredentials.Login.Contains("yandex"))
            {
                return new FromYandexGetter(client);
            }
            else
            {
                return new FromGmailGetter(client);
            }
        }
    }
}