namespace EmailWorker.Application.Exceptions;

public class EmailCredentialsGetterException : Exception
{
    public EmailCredentialsGetterException()
    {
    }

    public EmailCredentialsGetterException(string message)
        : base(message)
    {
    }

    public EmailCredentialsGetterException(string message, Exception inner)
        : base(message, inner)
    {
    }
}