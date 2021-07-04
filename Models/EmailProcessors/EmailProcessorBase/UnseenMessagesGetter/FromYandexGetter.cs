using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase.UnseenMessagesGetter
{
    public class FromYandexGetter : IGetter
    {
        public async Task<IList<object>> GetUnseenMessagesAsync(IEmailProcessorModel model)
        {
            model.Client.Connect(model.EmailCredentials.MailServer,
                                 model.EmailCredentials.Port,
                                 model.EmailCredentials.Ssl);
            model.Client.AuthenticationMechanisms.Remove("XOAUTH2");
            model.Client.Authenticate(model.EmailCredentials.Login, model.EmailCredentials.Password);
            model.Client.Inbox.Open(FolderAccess.ReadWrite);
            int messageCount = model.Client.Inbox.Count;
            IList<IMessageSummary> messageSummaries =
                await model.Client.Inbox.FetchAsync(0, messageCount,MessageSummaryItems.All
            );
            List<object> indexes = new();
            foreach (var item in messageSummaries)
            {
                indexes.Add(item.Index);
            }
            return indexes;
        }
    }
}