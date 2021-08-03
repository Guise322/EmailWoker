using System.Collections.Generic;
using System.IO;
using System.Net;
using EmailWorker.ApplicationCore.Interfaces.ProcessedMessageHandlers;

namespace EmailWorker.Infrastructure.EmailProcessor.ProcessedMessagesHandlers
{
    public class PublicIpGetterMessagesHandler : IProcessedMessageHandler
    {
        public (string emailText, string emailSubject) HandleProcessedMessages(
            List<object> messages)
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