using LicenseService.Domain.Models;

namespace LicenseService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> Login(string email, string password);
}
