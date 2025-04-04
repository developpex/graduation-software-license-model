using LicenseManager.Domain.Helpers;
using LicenseManager.Domain.Interfaces;

namespace LicenseManager.Infrastructure;

public class CertificateRepository : ICertificateRepository
{
    private readonly ICertificateHelper _certificateHelper;
    private readonly IRsaHelper _rasHelper;

    public CertificateRepository(ICertificateHelper certificateHelper, IRsaHelper rasHelper)
    {
        _certificateHelper = certificateHelper;
        _rasHelper = rasHelper;
    }

    public string GetCertificate()
    {
        var certificate = _certificateHelper.GetCertificate();
        return _rasHelper.ReadCertificate(certificate);
    }

    public void GetCertificateFromLicenseService()
    {
        _certificateHelper.GetCertificateFromLicenseService();
    }
}