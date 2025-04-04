using LicenseService.Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;

namespace LicenseService.Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<DbLicense> Licenses => Set<DbLicense>();
    public DbSet<DbUser> Users => Set<DbUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbLicense>(
            license =>
            {
                license.ToTable("license");
                license.HasKey(l => l.Id);
            });

        modelBuilder.Entity<DbUser>(
            license =>
            {
                license.ToTable("user");
                license.HasKey(u => u.Id);
            });
    }
}