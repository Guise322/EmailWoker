using System.Collections.Generic;
using MimeKit;

namespace EmailWorker.ApplicationCore.Entities
{
    public class EmailEntity
    {
        public EmailEntity(EmailCredentials emailCredentials)
        {
            EmailCredentials = emailCredentials;
        }
        public EmailCredentials EmailCredentials { get; }
        public string MyEmail { get; } = "guise322@yandex.ru";
        public List<object> UnseenMessages { get; set; }
        public List<object> ProcessedMessages { get; set; }
        public MimeMessage Message { get; set; }
    }
}