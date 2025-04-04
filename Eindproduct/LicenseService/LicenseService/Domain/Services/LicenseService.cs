using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;

namespace LicenseService.Domain.Services;

public class LicenseService : ILicenseService
{
    private readonly ILicenseRepository _licenseRepository;
    private readonly ICertificateService _certificateService;
    private readonly IEmailService _emailService;

    public LicenseService(ILicenseRepository licenseRepository,
        ICertificateService certificateService,
        IEmailService emailService)
    {
        _licenseRepository = licenseRepository;
        _certificateService = certificateService;
        _emailService = emailService;
    }

    public Task<IEnumerable<License>> GetLicensesAsync() =>
        _licenseRepository.GetLicensesAsync();

    public async Task<License> UpdateLicensePaymentAsync(string company)
    {
        var updatedLicense = await _licenseRepository.UpdateLicensePaymentAsync(company);
        _certificateService.CreateCertificate(updatedLicense);
        _emailService.SendUpdatePayment(updatedLicense);

        return updatedLicense;
    }

    public Task<License> GetLicenseAsync(string company) =>
        _licenseRepository.GetLicenseAsync(company);

    public async Task<License?> UpdateLicenseAsync(string company, int amount)
    {
        var updatedLicense = await _licenseRepository.UpdateLicenseAsync(company, amount);
        _certificateService.CreateCertificate(updatedLicense);
        _emailService.SendUpdateLicenseAmountEmail(updatedLicense);

        return updatedLicense;
    }
}
