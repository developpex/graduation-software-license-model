using LicenseManager.Domain.Exceptions;
using LicenseManager.Domain.Interfaces;
using FileNotFoundException = System.IO.FileNotFoundException;

namespace LicenseManager.Infrastructure.Helpers;

public class CertificateHelper : ICertificateHelper
{
    private readonly string _certificateLocation;
    private readonly string _company;

    public CertificateHelper(IConfiguration configuration)
    {
        _certificateLocation = configuration.GetSection("Certificate:Location").Value
                               ?? throw new NotFoundException("Appsetting",
                                   "certificate location path");
        _company = configuration.GetSection("Company:Name").Value
                   ?? throw new NotFoundException("Appsetting",
                       "company name");
    }

    public string GetCertificate()
    {
        var certificate = string.Empty;

        try
        {
            var files = Directory.GetFiles(_certificateLocation);
            foreach (var file in files)
            {
                if (Path.GetFileName(file).ToLower() != $"{_company}.txt") continue;

                certificate = file;
                break;
            }
        }
        catch (FileNotFoundException)
        {
        }

        return certificate;
    }

    public async void GetCertificateFromLicenseService()
    {
        using var httpClient = new HttpClient();
        using var response =
            await httpClient.GetAsync("https://localhost:44363/certificate?company=" + _company);
        response.EnsureSuccessStatusCode();
        var fileBytes = await response.Content.ReadAsByteArrayAsync();

        MoveExistingFileToOld();
        await File.WriteAllBytesAsync($"{_certificateLocation}\\{_company}.txt", fileBytes);
    }

    private void MoveExistingFileToOld()
    {
        var certificate = $"{_certificateLocation}\\{_company}.txt";
        var oldCertificate = $"{_certificateLocation}\\{_company}.txt.old";

        if (File.Exists(oldCertificate))
        {
            File.Delete(oldCertificate);
        }

        if (File.Exists(certificate))
        {
            File.Move(certificate, oldCertificate);
        }
    }
}
