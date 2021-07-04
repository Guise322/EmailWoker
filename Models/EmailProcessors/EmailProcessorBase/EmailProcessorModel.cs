using System.Collections.Generic;
using EmailWorker.Shared;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public class EmailProcessorModel : IEmailProcessorModel
    {
        public ImapClient Client { get; }
        public EmailCredentials EmailCredentials { get; }
        public string MyEmail { get; } = "guise322@yandex.ru";
        public IList<object> UnseenMessages { get; set; }
        public MimeMessage Message { get; set; }
    }
}