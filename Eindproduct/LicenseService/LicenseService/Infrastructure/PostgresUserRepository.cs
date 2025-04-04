using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;
using LicenseService.Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Infrastructure;

public class PostgresUserRepository : IUserRepository
{
    private readonly DatabaseContext _databaseContext;
    public PostgresUserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<User> Login(string email, string password)
    {
        var dbUser = await GetUser(email);
        if (dbUser.Password != password)
        {
            throw new UnsuccessfulLoginException();
        }

        return ConvertDbToDomain(dbUser);
    }

    private async Task<DbUser> GetUser(string email)
    {
        DbUser dbUser;
        try
        {
            dbUser = await _databaseContext.Users.Where(u => u.Email == email).FirstAsync();
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException("Email", email);
        }

        return dbUser;
    }

    private static User ConvertDbToDomain(DbUser dbUser)
    {
        return new User(
            dbUser.Name,
            dbUser.LastName,
            dbUser.Email,
            dbUser.Company,
            dbUser.Role
        );
    }
}