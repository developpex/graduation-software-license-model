using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LicenseService.UnitTests;

public class WatchdogHelperUnitTests
{
    [Fact]
    public async Task CheckConnection_SuccessStatusCodeShouldBeFalse_WhenConnectionIsOffline()
    {
        var client = new HttpClient();
        Func<Task> act = async () => await client.GetAsync("https://localhost:44356/watchdog");
        await act.Should().ThrowExactlyAsync<HttpRequestException>();
    }
}