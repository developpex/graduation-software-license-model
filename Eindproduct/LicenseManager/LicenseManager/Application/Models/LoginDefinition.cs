namespace LicenseManager.Application.Models;

public class LoginDefinition
{
    public LoginDefinition(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }
    public string Password { get; set; }
}