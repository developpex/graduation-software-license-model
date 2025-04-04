using System.ComponentModel.DataAnnotations.Schema;

namespace LicenseService.Infrastructure.DbModels;

[Table("license")]
public class DbLicense
{
    public DbLicense(int id, string company, int actualAmount, DateOnly lastPayment, DateOnly expiryDate)
    {
        Id = id;
        Company = company;
        ActualAmount = actualAmount;
        LastPayment = lastPayment;
        ExpiryDate = expiryDate;
    }
    
    [Column("id")] public int Id { get; set; }
    [Column("company")] public string Company { get; set; }
    [Column("actualamount")] public int ActualAmount { get; set; }
    [Column("lastpayment")] public DateOnly LastPayment { get; set; }
    [Column("expirydate")] public DateOnly ExpiryDate { get; set; }
}
