namespace LicenseManager.Domain.Interfaces;

public interface ICertificateHelper
{
    string GetCertificate();
    void GetCertificateFromLicenseService();
}
