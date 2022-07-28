namespace EmailWorker.Application;

internal static class EmailCredentialsValidator
{
    internal static bool Validate(EmailCredentials emailCredentials)
    {
        bool isStringsNullOrEmpty =
            IsEmailCredentialsStringsNullOrEmpty(
                emailCredentials.Login,
                emailCredentials.MailServer,
                emailCredentials.Password
        );
        return !(isStringsNullOrEmpty || emailCredentials.Port == 0);
    }

    private static bool IsEmailCredentialsStringsNullOrEmpty(
        string login,
        string mailServer,
        string password
    ) =>
        string.IsNullOrEmpty(login) ||
        string.IsNullOrEmpty(mailServer) ||
        string.IsNullOrEmpty(password);
}