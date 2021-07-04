using System.Collections.Generic;
using EmailWorker.Shared;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public interface IEmailProcessorModel
    {
        ImapClient Client { get; }
        EmailCredentials EmailCredentials { get; }
        string MyEmail { get; }
        IList<object> UnseenMessages { get; set; }
        MimeMessage Message { get; set; }
    }
}