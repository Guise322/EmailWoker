using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public class MessagesProcessor
    {
        public static bool ProcessMessages(IEmailProcessorModel model)
        {
            if(model.UnseenMessages.Count == 0)
            {
                return false;
            }

            int emailsOnRequest = 5;

            if (model.UnseenMessages.Count > emailsOnRequest)
            {
                if (model.UnseenMessages is IList<UniqueId>)
                {
                    for (int i = 0; i < emailsOnRequest; i++)
                    {
                        model.Client.Inbox.AddFlags((UniqueId)model.UnseenMessages[i],
                                                     MessageFlags.Seen, true);   
                    }
                }
                else
                {
                    for (int i = 0; i < emailsOnRequest; i++)
                    {
                        model.Client.Inbox.AddFlags((int)model.UnseenMessages[i],
                                                     MessageFlags.Seen, true);   
                    }
                }
                model.Client.Disconnect(true); //check when the client was connected
            }

            return true;
        }
    }
}