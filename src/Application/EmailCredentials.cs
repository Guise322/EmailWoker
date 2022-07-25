using EmailWorker.Application.Enums;

namespace EmailWorker.Application;

public record EmailCredentials(
    string MailServer,
    int Port,
    bool Ssl,
    string Login,
    string Password,
    DedicatedWorkType DedicatedWork
);