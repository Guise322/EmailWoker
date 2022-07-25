using MimeKit;

namespace EmailWorker.Application.Interfaces;

public interface IReportSender
{
    void SendReportViaSmtp(MimeMessage message, EmailCredentials emailCredentials);
}