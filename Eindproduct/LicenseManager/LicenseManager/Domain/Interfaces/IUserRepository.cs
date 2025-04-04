using LicenseManager.Domain.Models;

namespace LicenseManager.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> Login(string email, string password, License license);
}
