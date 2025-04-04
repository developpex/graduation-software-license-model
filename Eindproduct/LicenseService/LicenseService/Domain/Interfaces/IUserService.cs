namespace LicenseService.Domain.Interfaces;

public interface IUserService
{
    Task<string> Login(string email, string password);
}
