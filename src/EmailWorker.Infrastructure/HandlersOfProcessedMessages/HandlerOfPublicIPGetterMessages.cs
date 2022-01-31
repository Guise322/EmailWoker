//Some of the below lines of code is got from the site:
//https://qawithexperts.com/article/c-sharp/get-ip-address-using-c-local-and-public-ip-example/374

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfPublicIPGetterMessages : IHandlerOfPublicIPGetterMessages
    {
        private readonly string ipRequestAddress = "http://checkip.dyndns.org/";
        private readonly ILogger<HandlerOfPublicIPGetterMessages> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HandlerOfPublicIPGetterMessages(
            ILogger<HandlerOfPublicIPGetterMessages> logger,
            IHttpClientFactory httpClientFactory) =>
            (_logger, _httpClientFactory) = (logger, httpClientFactory);
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            string address;
            HttpRequestMessage request = new(HttpMethod.Get,ipRequestAddress);
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = httpClient.Send(request);
            address = response.Content.ToString();

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);
            
            _logger.LogInformation("Getting of the current succeeds.");

            return (address, "Ip By Email Project");
        }
    }
}