namespace EmailWorker.ApplicationCore.Interfaces.ServiceContexts
{
    public interface IPublicIPGetterContext : IEmailProcessorServiceContext
    {
        IMessageGetter MessageGetter { get; set; }
        IPublicIPGetter IPGetter { get; set; }
    }
}