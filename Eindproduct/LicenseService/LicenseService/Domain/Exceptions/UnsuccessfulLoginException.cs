namespace LicenseService.Domain.Exceptions;

public class UnsuccessfulLoginException : Exception
{
    public UnsuccessfulLoginException()
        : base("Incorrect password")
    {
    }
}