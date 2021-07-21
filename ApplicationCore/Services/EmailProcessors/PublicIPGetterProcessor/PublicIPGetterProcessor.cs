using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using MimeKit;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor
{
    public class PublicIPGetterProcessor : EmailProcessor
    {
        private PublicIPGetterServices Services { get; }
        public PublicIPGetterProcessor(EmailEntity emailEntity, PublicIPGetterServices services)
            : base (emailEntity, services)
        {
            Services = services;
        }
        public override List<object> ProcessMessages(List<object> messages) =>
            MessagesProcessor.ProcessMessages(messages, MyEmail, Services.MessageGetter);
        public override MimeMessage BuildAnswerMessage(List<object> messages) =>
            AnswerBuilder.BuildAnswerMessages(EmailEntity.EmailCredentials, MyEmail, Services.IPGetter);
    }
}