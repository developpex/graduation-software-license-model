namespace LicenseService.Domain.Models;

public class License
{
    public License(string company, int actualAmount, DateOnly lastPayment, DateOnly expiryDate)
    {
        Company = company;
        ActualAmount = actualAmount;
        LastPayment = lastPayment;
        ExpiryDate = expiryDate;
    }

    public string Company { get; set; }
    public int ActualAmount { get; set; }
    public DateOnly LastPayment { get; set; }
    public DateOnly ExpiryDate { get; set; }
}