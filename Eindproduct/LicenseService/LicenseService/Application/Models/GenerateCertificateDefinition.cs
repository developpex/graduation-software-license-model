namespace LicenseService.Application.Models;

public class GenerateCertificateDefinition
{
    public GenerateCertificateDefinition(string company, int actualAmount)
    {
        Company = company;
        ActualAmount = actualAmount;
    }

    public string Company { get; set; }
    public int ActualAmount { get; set; }
}