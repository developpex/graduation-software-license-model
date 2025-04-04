using LicenseManager.Domain.Models;

namespace LicenseManager.Domain.Interfaces;

public interface IUserService
{
    Task<User> Login(string email, string password);
}
