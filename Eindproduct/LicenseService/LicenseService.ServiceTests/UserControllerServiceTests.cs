using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LicenseService.Application.Models;
using LicenseService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace LicenseService.ServiceTests;

public class UserControllerServiceTests : ApiServiceTestBase
{
    [Fact]
    public async Task Login_ShouldReturnStatus200OK_WhenCorrectLoginCredentialsAreGiven()
    {
        // Arrange
        using var client = CreateClient();
        var request = new LoginDefinition("admin@test.nl",
            "ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae");
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("https://localhost:44363/user", content );

        // Assert
        response.Should()
            .Be200Ok();
    }

    [Fact]
    public async Task Login_ShouldReturnStatus404NotFound_WhenInCorrectLoginNameIsGiven()
    {
        // Arrange
        using var client = CreateClient();
        var request = new LoginDefinition("NotFound",
            "ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae");
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("https://localhost:44363/user", content);

        // Assert
        response.Should()
            .Be404NotFound();
    }

    [Fact]
    public async Task Login_ShouldReturnStatus406NotAcceptable_WhenInCorrectPasswordIsGiven()
    {
        // Arrange
        using var client = CreateClient();
        var request = new LoginDefinition("admin@test.nl",
            "incorrect");
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("https://localhost:44363/user", content);

        // Assert
        response.Should()
            .Be406NotAcceptable();
    }
}