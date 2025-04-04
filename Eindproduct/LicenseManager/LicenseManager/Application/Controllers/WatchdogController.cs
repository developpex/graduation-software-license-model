using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace LicenseManager.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class WatchdogController : ControllerBase
{
    /// <summary>
    /// Watchdog
    /// </summary>
    /// <response code="200">Connection</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public Task<IActionResult> CheckConnection()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}
