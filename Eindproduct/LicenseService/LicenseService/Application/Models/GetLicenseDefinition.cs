namespace LicenseService.Application.Models;

public class GetLicenseDefinition
{
    public GetLicenseDefinition(string company)
    {
        Company = company;
    }

    public string Company { get; set; }
}