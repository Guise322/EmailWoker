using MimeKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public class AnswerMessageBuilder
    {
        public static void BuildAnswerMessage(IEmailProcessorModel model)
        {
            MimeMessage message = FromToBuilder.BuildFromTo(model);
            message.Subject = "The count of messages marked as seen";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The count of seen messages equals {0}",
                                     model.UnseenMessages.Count)
            };
            model.Message = message;
        }
    }
}