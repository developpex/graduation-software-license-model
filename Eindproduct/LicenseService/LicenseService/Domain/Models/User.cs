namespace LicenseService.Domain.Models;

public class User
{
    public User(string name, string lastName, string email, string company, string role)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Company = company;
        Role = role;
    }

    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Company { get; set; }
    public string Role { get; set; }
}