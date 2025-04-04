namespace LicenseService.Domain.Interfaces;

public interface IRsaHelper
{
    string SignCertificate(string text);
}
