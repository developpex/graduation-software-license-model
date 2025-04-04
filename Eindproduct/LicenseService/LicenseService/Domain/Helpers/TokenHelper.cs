using System.IdentityModel.Tokens.Jwt;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;

namespace LicenseService.Domain.Helpers;

public class TokenHelper : ITokenHelper
{
    public Dictionary<string, string> GetAllClaims(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        return jwtToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
    }

    public string GetClaimValue(string token, string type)
    {
        var claims = GetAllClaims(token);

        return claims.TryGetValue(type, out var value)
            ? value
            : throw new NotFoundException("Dictionary type", type);
    }
}