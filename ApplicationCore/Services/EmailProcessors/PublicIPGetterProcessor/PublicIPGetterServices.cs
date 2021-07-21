using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor
{
    public class PublicIPGetterServices : EmailServicesBase
    {
        public IMessageGetter MessageGetter { get; set; }
        public IPublicIPGetter IPGetter { get; set; }
    }
}