using System.ComponentModel;
using System.Net.Mime;
using LicenseManager.Domain.Exceptions;
using LicenseManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LicenseManager.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService,
        IConfiguration configuration)
    {
        _certificateService = certificateService;
    }

    /// <summary>
    /// Retrieve certificate for a company on the server
    /// </summary>
    /// <returns>Certificate</returns>
    /// <response code="200">License info</response>
    /// <response code="404">License not found</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(License))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCertificate()
    {
        try
        {
            return Ok(await _certificateService.GetCertificate());
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
        catch (UnableToVerifySignatureException exception)
        {
            return Problem(statusCode: 406, detail: exception.Message);
        }
    }

    /// <summary>
    /// Retrieve certificate for a company from the licenseService
    /// </summary>
    /// <returns>Certificate</returns>
    /// <response code="200"></response>
    /// <response code="404">License not found</response>
    [HttpGet("/LicenseService")]
    [Produces(MediaTypeNames.Text.Plain)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetCertificateFromLicenseService()
    {
        try
        {
            _certificateService.GetCertificateFromLicenseService();
            return Task.FromResult<IActionResult>(Ok());
        }
        catch (NotFoundException exception)
        {
            return Task.FromResult<IActionResult>(Problem(statusCode: 404, detail: exception.Message));
        }
    }
}
