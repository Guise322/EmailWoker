using System.IO;
using System.Net;
using EmailWorker.ApplicationCore.Interfaces;

namespace EmailWorker.Infrastructure.EmailServicesImplementations
{
    public class PublicIPGetter : IPublicIPGetter
    {
        public string GetPublicIP()
        {
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

            return address;
        }
    }
}