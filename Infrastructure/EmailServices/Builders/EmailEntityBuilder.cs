using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.Infrastructure.EmailServices.Builders
{
    public class EmailEntityBuilder
    {
        public static EmailEntity BuildEmailEntity(EmailCredentials emailCredentials)
        {
            return new EmailEntity(emailCredentials);
        }
    }
}