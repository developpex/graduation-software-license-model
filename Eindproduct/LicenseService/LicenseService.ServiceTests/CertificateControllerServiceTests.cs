using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LicenseService.ServiceTests;

public class CertificateControllerServiceTests : ApiServiceTestBase
{

    [Fact]
    public async Task GetCertificate_ShouldReturnStatus200OK_WhenCorrectCompanyIsGiven()
    {
        // Arrange
        var company = "tbwb";
        using var client = CreateClient();

        // Act
        var response = await client.GetAsync("https://localhost:44363/certificate?company=" + company);

        // Assert
        response.Should()
            .Be200Ok();
    }

    [Fact]
    public async Task GetCertificate_ShouldReturnStatus404NotFound_WhenIncorrectCompanyIsGiven()
    {
        // Arrange
        var company = "incorrect";
        using var client = CreateClient();

        // Act
        var response =
            await client.GetAsync("https://localhost:44363/certificate?company=" + company);

        // Assert
        response.Should().Be500InternalServerError();
    }
}