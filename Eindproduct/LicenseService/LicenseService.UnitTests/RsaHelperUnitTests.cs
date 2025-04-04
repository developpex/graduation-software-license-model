using System.Collections.Generic;
using FluentAssertions;
using LicenseService.Domain.Helpers;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace LicenseService.UnitTests;

public class RsaHelperUnitTests
{
    [Fact]
    public void CreateBearerToken_ShouldCreateValidToken()
    {
        // Arrange
        var myConfiguration = new Dictionary<string, string>
        {
            {"Certificate:PrivateKey",
                "C:\\Users\\Patrick\\Desktop\\Afstudeer Opdracht\\Eindproduct\\LicenseService\\PrivateKey\\privatekey.pem"
            },
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

        var rsaHelper = new RsaHelper(configuration);
        const string textToSign = "TextToSign";

        // Act
        var signature = rsaHelper.SignCertificate(textToSign);

        // Assert
        signature.Should().NotBeNullOrEmpty();
    }
}
