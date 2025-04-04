namespace LicenseManager.Domain.Exceptions;

public class ExpiryDateException : Exception
{
    public ExpiryDateException()
        : base("exceeding expiration date on the license, please contact sales department of TBWB")
    {
    }
}
