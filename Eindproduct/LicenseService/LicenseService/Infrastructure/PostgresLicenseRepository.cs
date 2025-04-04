using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;
using LicenseService.Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Infrastructure;

public class PostgresLicenseRepository : ILicenseRepository
{
    private readonly DatabaseContext _databaseContext;

    public PostgresLicenseRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IEnumerable<License>> GetLicensesAsync()
    {
        return await _databaseContext.Licenses.OrderBy(l => l.Company)
            .Select(l => ConvertDbToDomain(l))
            .ToArrayAsync();
    }

    public async Task<License> UpdateLicensePaymentAsync(string company)
    {
        var newDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        var dbLicense = await GetDbLicenseAsync(company);
        dbLicense.LastPayment = newDate;
        dbLicense.ExpiryDate = newDate.AddMonths(1);

        _databaseContext.Licenses.Update(dbLicense);
        await _databaseContext.SaveChangesAsync();

        return ConvertDbToDomain(dbLicense);
    }

    public async Task<License> GetLicenseAsync(string company)
    {
        return ConvertDbToDomain(await GetDbLicenseAsync(company));
    }

    public async Task<License?> UpdateLicenseAsync(string company, int amount)
    {
        var dbLicense = await GetDbLicenseAsync(company);
        dbLicense.Company = company;
        dbLicense.ActualAmount = amount;

        _databaseContext.Licenses.Update(dbLicense);
        await _databaseContext.SaveChangesAsync();

        return ConvertDbToDomain(dbLicense);
    }

    private async Task<DbLicense> GetDbLicenseAsync(string company)
    {
        DbLicense dbLicense;
        try
        {
            dbLicense = await _databaseContext.Licenses
                .Where(l => l.Company == company)
                .FirstAsync();
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException("Company", company);
        }

        return dbLicense;
    }

    private static License ConvertDbToDomain(DbLicense dbLicense)
    {
        return new License(
            dbLicense.Company,
            dbLicense.ActualAmount,
            dbLicense.LastPayment,
            dbLicense.ExpiryDate
        );
    }
}
