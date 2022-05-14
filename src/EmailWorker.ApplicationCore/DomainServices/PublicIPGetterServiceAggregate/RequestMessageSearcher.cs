using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;

public class RequestMessageSearcher : IRequestMessageSearcher
{
    private IMessageGetter MessageGetter { get; set; }
    public RequestMessageSearcher(IMessageGetter messageGetter) =>
        MessageGetter = messageGetter;
    public List<UniqueId> SearchRequestMessage(IList<UniqueId> messageIDs, string searchedEmail) =>
        messageIDs.Where(messageID => 
        {
            Guard.Against.NullOrEmpty(searchedEmail, nameof(searchedEmail));
            MimeMessage messageFromBox = MessageGetter.GetMessage(messageID);
            string rawEmailFrom = messageFromBox.From.ToString();
            string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);
            return emailFrom == searchedEmail;
        }).ToList();
}