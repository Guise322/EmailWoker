namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase.UnseenMessagesGetter
{
    public class GetterBuilder
    {
        public static IGetter BuildGetter(IEmailProcessorModel model)
        {
            if (model.EmailCredentials.Login.Contains("ya") ||
                model.EmailCredentials.Login.Contains("yandex"))
            {
                return new FromYandexGetter();
            }
            else
            {
                return new FromGmailGetter();
            }
        }
    }
}