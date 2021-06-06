using System;

namespace EmailWorker.Shared
{
    public enum DedicatedWorks
    {
        SearchRequest, MarkAsSeen
    }
    public enum EmailBoxes
    {
        Yandex, Google, Another
    }
    public class EmailCredentials
    {
        public string MailServer {get; set;}
        public int Port {get; set;}
        public bool Ssl {get; set;}
        public string Login {get; set;}
        public string Password {get; set;}
        //[JsonConverter(typeof(StringEnumConverter))]
        public DedicatedWorks DedicatedWork {get; set;}
    }
}