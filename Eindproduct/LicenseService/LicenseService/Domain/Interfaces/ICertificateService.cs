using LicenseService.Domain.Models;

namespace LicenseService.Domain.Interfaces;

public interface ICertificateService
{
    void CreateCertificate(License license);
}
