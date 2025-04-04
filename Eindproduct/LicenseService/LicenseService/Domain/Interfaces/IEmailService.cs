using LicenseService.Domain.Models;

namespace LicenseService.Domain.Interfaces;

public interface IEmailService
{
    void SendUpdateLicenseAmountEmail(License license);
    void SendUpdatePayment(License license);
    void SendWatchdogEmail();
}
