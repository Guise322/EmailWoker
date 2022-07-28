using EmailWorker.Application;
using EmailWorker.Application.Enums;

namespace EmailWorker.Tests.Integration;

internal static class TestEmailCredentialsFactory
{
    internal static EmailCredentials Create() =>
        new (
            MailServer: "test",
            Port: 111,
            Ssl: true,
            Login: "test",
            Password: "test",
            DedicatedWork: DedicatedWorkType.MarkAsSeen
        );
}