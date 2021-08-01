namespace EmailWorker.ApplicationCore.Interfaces.ServiceContexts
{
    public interface IMarkAsSeenContext : IEmailProcessorServiceContext
    {
        IAnswerSender AnswerSender { get; set; }
        IUnseenMessagesGetter UnseenMessagesGetter { get; set; }
        IProcessedMessagesHandler ProcessedMessagesHandler { get; set; }
        IClientConnector ClientConnector { get; set; }
    }
}