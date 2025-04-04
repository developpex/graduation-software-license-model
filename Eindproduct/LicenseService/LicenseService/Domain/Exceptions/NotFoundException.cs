namespace LicenseService.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string valueName, string value)
        : base($"{valueName} {value} not found")
    {
    }
}