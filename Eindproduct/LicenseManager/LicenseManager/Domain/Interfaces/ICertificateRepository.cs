namespace LicenseManager.Domain.Interfaces;

public interface ICertificateRepository
{
    string GetCertificate();
    void GetCertificateFromLicenseService();
}