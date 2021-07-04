using System.Threading.Tasks;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase.UnseenMessagesGetter
{
    public class InboxGetter
    {
        public static async Task GetUnseenMessagesAsync(IEmailProcessorModel model)
        {
            model.UnseenMessages = await GetterBuilder.BuildGetter(model)
                .GetUnseenMessagesAsync(model);
        }
    }
}