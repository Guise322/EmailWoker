using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces;

public interface IReportSender
{
    void SendReportViaSmtp(MimeMessage message, EmailCredentials emailCredentials);
}