using EmailWorker.ApplicationCore.Enums;

namespace EmailWorker.ApplicationCore.Entities
{
    public class EmailCredentials
    {
        public string MailServer { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DedicatedWorkType DedicatedWork { get; set; }
    }
}