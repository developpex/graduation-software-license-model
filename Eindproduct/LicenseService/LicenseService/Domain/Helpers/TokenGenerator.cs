using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;

namespace LicenseService.Domain.Helpers;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("name", user.Name),
            new("lastname", user.LastName),
            new("email", user.Email),
            new("company", user.Company),
            new("role", user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtBearer:TokenSecret"] ??
                                   throw new NotFoundException("Appsetting", "token secret")));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtBearer:Issuer"],
            audience: _configuration["JwtBearer:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
