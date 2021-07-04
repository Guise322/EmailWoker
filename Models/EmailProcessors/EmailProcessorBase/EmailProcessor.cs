using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.Shared;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public class EmailProcessor : IEmailProcessor
    {
        protected IEmailProcessorModel Model { get; }
        public EmailProcessor(IEmailProcessorModel model)
        {
            Model = model;
        }
        public async Task GetUnseenMessagesAsync() =>
            await UnseenMessagesGetter.InboxGetter.GetUnseenMessagesAsync(Model);
        public virtual bool ProcessMessages()
        {
            return MessagesProcessor.ProcessMessages(Model);
        }
        public virtual EmailProcessor BuildAnswerMessage()
        {
            AnswerMessageBuilder.BuildAnswerMessage(Model);
            return this;
        }
        public void SendAnswerBySmtp() =>
            AnswerSender.SendAnswerBySmtp(Model);
        public async Task ProcessEmailBoxAsync()
        {
            await GetUnseenMessagesAsync();
            if(ProcessMessages())
            {
                BuildAnswerMessage().SendAnswerBySmtp();
            }
        }
    }
}