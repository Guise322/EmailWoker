using EmailWorker.Controllers.EmailProcessors.PublicIPGetterProcessor;
using EmailWorker.Models.EmailProcessors.EmailProcessorBase;
using EmailWorker.Shared;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBuilder
{
    public class Builder
    {
        public static IEmailProcessor BuildEmailProcessor(EmailCredentials emailCredentials) =>
            emailCredentials.DedicatedWork switch
                {
                    DedicatedWorkType.MarkAsSeen =>
                        new EmailProcessor(BuildEmailProcessorModel()),
                    DedicatedWorkType.SearchRequest =>
                        new PublicIPGetterProcessor(BuildPublicIPGetterProcessorModel()),
                    _ => null
                };
        private static IEmailProcessorModel BuildEmailProcessorModel()
        {
            return new EmailProcessorModel();
        }
        private static IEmailProcessorModel BuildPublicIPGetterProcessorModel()
        {
            return new EmailProcessorModel();
        }
    }
}