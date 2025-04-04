namespace LicenseManager.Domain.Exceptions;

public class UnableToVerifySignatureException : Exception
{
    public UnableToVerifySignatureException(string file)
        : base($"Could not verify signature on file: {file}")
    {
    }
}