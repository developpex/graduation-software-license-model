using System.ComponentModel.DataAnnotations.Schema;

namespace LicenseService.Infrastructure.DbModels;

[Table("user")]
public class DbUser
{
    public DbUser(string name, string lastName, string email, string password, string company, string role)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Password = password;
        Company = company;
        Role = role;
    }
    
    [Column("id")]
    public int Id { get; set; }

    [Column("name")] 
    public string Name { get; set; }

    [Column("lastname")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("company")]
    public string Company { get; set; }

    [Column("role")]
    public string Role { get; set; }
}
