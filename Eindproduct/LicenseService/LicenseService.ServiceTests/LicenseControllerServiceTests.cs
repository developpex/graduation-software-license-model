using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace LicenseService.ServiceTests;

public class LicenseControllerServiceTests : ApiServiceTestBase
{
    public string JwtToken { get; set; }

    [Fact]
    public async Task GetLicense_ShouldReturnStatus200OK_WhenCorrectCompanyIsGiven()
    {
        // Arrange
        CreateJwToken("tbwb");
        using var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", JwtToken);

        // Act
        var response = await client.GetAsync("https://localhost:44363/license");

        // Assert
        response.Should()
            .Be200Ok();
    }

    [Fact]
    public async Task GetLicense_ShouldReturnStatus401NofFound_WhenIncorrectCompanyIsGiven()
    {
        // Arrange
        CreateJwToken("incorrect");
        using var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", JwtToken);

        // Act
        var response = await client.GetAsync("https://localhost:44363/license");

        // Assert
        response.Should().Be404NotFound();
    }

    private void CreateJwToken(string company)
    {
        var claims = new List<Claim>
        {
            new("company", company)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            "NjOZM^2gc17sSCEbXwVKMzYn9$f&rjP%"));

        var token = new JwtSecurityToken(
            issuer: "TBWB/auth",
            audience: "TBWB clients",
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
    }
}