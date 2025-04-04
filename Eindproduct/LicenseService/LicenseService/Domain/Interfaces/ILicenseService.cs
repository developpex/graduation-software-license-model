using LicenseService.Domain.Models;

namespace LicenseService.Domain.Interfaces;

public interface ILicenseService
{
    Task<License> GetLicenseAsync(string company);
    Task<License?> UpdateLicenseAsync(string company, int amount);
    Task<IEnumerable<License>> GetLicensesAsync();
    Task<License> UpdateLicensePaymentAsync(string company);
}