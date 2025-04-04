using LicenseManager.Domain.Models;

namespace LicenseManager.Domain.Interfaces;

public interface ICertificateService
{
    Task<License?> GetCertificate();
    void GetCertificateFromLicenseService();
}
