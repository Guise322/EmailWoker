using MailKit.Net.Smtp;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public class AnswerSender
    {
        public static void SendAnswerBySmtp(IEmailProcessorModel model)
        {
            int smtpPort = 465;

            using var client = new SmtpClient();
            client.Connect(model.EmailCredentials.MailServer, smtpPort, model.EmailCredentials.Ssl);
            client.Authenticate(model.EmailCredentials.Login, model.EmailCredentials.Password);
            client.Send(model.Message);
            client.Disconnect(true);
        }
    }
}