using MimeKit;

namespace EmailWorker.Application.Interfaces;

public interface IReportSender
{
    void SendReportViaSmtp(
        string emailToReport,
        string emailSubject,
        string emailText,
        EmailCredentials emailCredentials
    );
}