namespace LicenseManager.Domain.Exceptions;

public class ExceedLicenseAmountException : Exception
{
    public ExceedLicenseAmountException()
        : base("Unable to log in, exceeding license amount")
    {
    }
}