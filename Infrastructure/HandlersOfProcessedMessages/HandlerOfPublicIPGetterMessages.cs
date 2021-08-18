using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfPublicIpGetterMessages : IHandlerOfPublicIPGetterMessages
    {
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            if (messages == null)
            {
                return (null, null);
            }

            string address;
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return (address, "Ip By Email Project");
        }
    }
}