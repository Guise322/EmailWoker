using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit;
using MailKit.Search;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase.UnseenMessagesGetter
{
    public class FromGmailGetter : IGetter
    {
        public async Task<IList<object>> GetUnseenMessagesAsync(IEmailProcessorModel model)
        {
            model.Client.Connect(model.EmailCredentials.MailServer,
                                 model.EmailCredentials.Port,
                                 model.EmailCredentials.Ssl);
            model.Client.AuthenticationMechanisms.Remove("XOAUTH2");
            model.Client.Authenticate(model.EmailCredentials.Login, model.EmailCredentials.Password);
            model.Client.Inbox.Open(FolderAccess.ReadWrite);
            SearchResults unseenMessages = await model.Client.Inbox
                .SearchAsync(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
            return (IList<object>) unseenMessages.UniqueIds;
        }
    }
}