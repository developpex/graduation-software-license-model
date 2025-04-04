namespace LicenseManager.Domain.Models;

public class User
{
    public User(string name, string lastName, string role)
    {
        Name = name;
        LastName = lastName;
        Role = role;
    }

    public string Name { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}
