using System.ServiceProcess;
using System.Text.Json;
using LicenseManager.Domain.Interfaces;
using LicenseManager.Domain.Models;

namespace LicenseManager.Domain.Services;

public class CertificateService : ServiceBase, ICertificateService
{
    private readonly ICertificateRepository _certificateRepository;

    public CertificateService(ICertificateRepository certificateRepository)
    {
        _certificateRepository = certificateRepository;
    }

    public Task<License?> GetCertificate()
    {
        var certificate = _certificateRepository.GetCertificate();
        return Task.FromResult(JsonSerializer.Deserialize<License>(certificate));
    }

    public void GetCertificateFromLicenseService() =>
        _certificateRepository.GetCertificateFromLicenseService();
}
