using LicenseService.Domain.Models;

namespace LicenseService.Domain.Interfaces;

public interface ITokenGenerator
{
    string GenerateAccessToken(User user);
}