namespace LicenseService.Domain.Interfaces;

public interface ITokenHelper
{
    Dictionary<string, string> GetAllClaims(string token);
    string GetClaimValue(string token, string type);
}
