using LicenseManager.Domain.Exceptions;
using LicenseManager.Domain.Interfaces;
using LicenseManager.Domain.Models;
using LicenseManager.Infrastructure.DbModels;

namespace LicenseManager.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly List<DbUser> _dbUsers;
    private readonly List<User> _loggedInUsers;

    public UserRepository()
    {
        _dbUsers = new List<DbUser>
        {
            new("patrick", "van Nieuwburg", "test@test.nl", "test123!", "picker"),
            new("service", "department", "service@test.nl", "test123!", "service"),
        };

        _loggedInUsers = new List<User>
        {
            new("user", "1", "picker"),
            new("user", "2", "picker"),
        };
    }

    public async Task<User> Login(string email, string password, License license)
    {
        var dbUser = await GetUser(email);
        if (dbUser.Password != password)
        {
            throw new UnsuccessfulLoginException();
        }

        if (dbUser.Role == "service")
        {
            return ConvertDbToDomain(dbUser);
        }

        if (license.ExpiryDate <
            new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
        {
            if (_loggedInUsers.Count >= 3)
            {
                throw new ExpiryDateException();
            }
        }

        if (license != null && license.ActualAmount <= _loggedInUsers.Count)
        {
            throw new ExceedLicenseAmountException();
        }

        var user = ConvertDbToDomain(dbUser);
        _loggedInUsers.Add(user);

        return user;
    }

    private Task<DbUser> GetUser(string email)
    {
        var dbUser = _dbUsers.Find(user => user.Email == email)!;
        if (dbUser == null)
        {
            throw new NotFoundException("Email", email);
        }

        return Task.FromResult(dbUser);
    }

    private static User ConvertDbToDomain(DbUser dbUser)
    {
        return new User(
            dbUser.Name,
            dbUser.LastName,
            dbUser.Role
        );
    }
}
