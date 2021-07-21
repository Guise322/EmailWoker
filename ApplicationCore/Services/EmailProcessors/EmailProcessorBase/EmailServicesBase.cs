using EmailWorker.ApplicationCore.Interfaces;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase
{
    public class EmailServicesBase
    {
        public IAnswerSender AnswerSender { get; set; }
        public IUnseenMessagesGetter UnseenMessagesGetter { get; set; }
        public IProcessedMessagesHandler ProcessedMessagesHandler { get; set; }
        public IClientConnector ClientConnector { get; set; }
    }
}