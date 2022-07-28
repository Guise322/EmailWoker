using EmailWorker.Application;
using EmailWorker.Application.Enums;
using Xunit;

namespace EmailWorker.Tests.Unit.Application;

public class EmailCredentialsValidatorTests
{
    [Theory]
    [InlineData(null, 1, true, "test", "test", DedicatedWorkType.MarkAsSeen)]
    [InlineData("test", 0, true, "test", "test", DedicatedWorkType.MarkAsSeen)]
    [InlineData("test", 1, true, null, "test", DedicatedWorkType.MarkAsSeen)]
    [InlineData("test", 1, true, "test", null, DedicatedWorkType.MarkAsSeen)]
    public void Validate_NullOrEmptyValues_False(
        string mailServer,
        int port,
        bool ssl,
        string login,
        string password,
        DedicatedWorkType dedicatedWork
    )
    {
        var emailCredentials = new EmailCredentials(mailServer, port, ssl, login, password, dedicatedWork);
    
        bool actualResult = EmailCredentialsValidator.Validate(emailCredentials);
        Assert.False(actualResult);
    }

    [Theory]
    [InlineData("test", 1, true, "test", "test", DedicatedWorkType.MarkAsSeen)]
    public void Validate_EmailCredentials_True(
        string mailServer,
        int port,
        bool ssl,
        string login,
        string password,
        DedicatedWorkType dedicatedWork
    )
    {
        var emailCredentials = new EmailCredentials(mailServer, port, ssl, login, password, dedicatedWork);
    
        bool actualResult = EmailCredentialsValidator.Validate(emailCredentials);
        Assert.True(actualResult);
    }
}