using System.Threading.Tasks;
using EmailWorker.Infrastructure.EmailServices;
using MailKit.Net.Imap;

namespace EmailWorker.Controllers
{
    public class EmailWorkerController
    {
        public static Task ProcessEmailsAsync()
        {
            ImapClient client = new();
            return EmailBoxHandler.HandleEmailBoxesAync(client);
        }
    }    
}