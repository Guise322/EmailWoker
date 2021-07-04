using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.Models;
using EmailWorker.Models.EmailProcessors.EmailProcessorBase;

namespace EmailWorker.Controllers.EmailProcessors.PublicIPGetterProcessor
{
    public class PublicIPGetterProcessor : EmailProcessor
    {
        public PublicIPGetterProcessor(IEmailProcessorModel model) : base (model)
        {

        }
        public override bool ProcessMessages()
        {
            return ResultsProcessor.ProcessResults(Model);
        }
        public override EmailProcessor BuildAnswerMessage()
        {
            Model.Message = AnswerBuilder.BuildAnswer(Model);
            return this;
        }
    }
}