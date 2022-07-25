namespace EmailWorker.Application.Interfaces;

public interface IEmailCredentialsGetter
{
    List<EmailCredentials> GetEmailCredentialsList();
}