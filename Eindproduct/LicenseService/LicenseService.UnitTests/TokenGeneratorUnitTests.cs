using System;
using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using LicenseService.Domain.Helpers;
using LicenseService.Domain.Models;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace LicenseService.UnitTests;

public class TokenGeneratorUnitTests
{
    [Fact]
    public void CreateBearerToken_ShouldCreateValidToken()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        configuration["JwtBearer:TokenSecret"].Returns("testTokenSecretThatIs128BitsLong");
        configuration["JwtBearer:Issuer"].Returns("testIssuer");
        configuration["JwtBearer:Audience"].Returns("testAudience");
        var tokenGenerator = new TokenGenerator(configuration);
        var user = new User("testName", "testLastName", "testEmail", "testCompany", "testRole");

        // Act
        var token = tokenGenerator.GenerateAccessToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.Replace("Bearer ", ""));
        jwtToken.Claims.Should().Contain(c => c.Type == "name" && c.Value == "testName");
        jwtToken.Claims.Should().Contain(c => c.Type == "lastname" && c.Value == "testLastName");
        jwtToken.Claims.Should().Contain(c => c.Type == "email" && c.Value == "testEmail");
        jwtToken.Claims.Should().Contain(c => c.Type == "company" && c.Value == "testCompany");
        jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == "testRole");
        jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }
}
