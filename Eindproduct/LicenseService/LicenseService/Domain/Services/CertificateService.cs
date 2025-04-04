using System.Text.Json;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;

namespace LicenseService.Domain.Services;

public class CertificateService : ICertificateService
{
    private readonly IRsaHelper _rsaHelper;
    private readonly string _certificateDirectory;

    public CertificateService(IConfiguration configuration, IRsaHelper rsaHelper)
    {
        _rsaHelper = rsaHelper;
        _certificateDirectory = configuration.GetSection("Certificate:Directory").Value
                                ?? throw new NotFoundException("Appsetting",
                                    "certificate directory");
    }

    public void CreateCertificate(License license)
    {
        var certificate = $"{_certificateDirectory}\\{license.Company}.txt";
        var oldCertificate = $"{_certificateDirectory}\\{license.Company}.txt.old";

        try
        {
            if (File.Exists(oldCertificate))
            {
                File.Delete(oldCertificate);
            }

            if (File.Exists(certificate))
            {
                File.Move(certificate, oldCertificate);
            }

            using var writer = new StreamWriter(certificate);
            writer.WriteLine(JsonSerializer.Serialize(license));
            writer.WriteLine(string.Empty);
            writer.WriteLine("--- START SIGNATURE ---");
            writer.WriteLine(_rsaHelper.SignCertificate(JsonSerializer.Serialize(license)));
            writer.WriteLine("--- END SIGNATURE ---");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
