using System.Net.Mime;
using LicenseService.Application.Models;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LicenseService.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificateController : ControllerBase
{
    private readonly string _certificateDirectory;
    private readonly ICertificateService _certificateService;
    private readonly ILicenseService _licenseService;

    public CertificateController(IConfiguration configuration,
        ICertificateService certificateService,
        ILicenseService licenseService)
    {
        _certificateService = certificateService;
        _licenseService = licenseService;
        _certificateDirectory = configuration.GetSection("Certificate:Directory").Value
                                ?? throw new NotFoundException("Appsetting",
                                    "certificate directory");
    }

    /// <summary>
    /// Retrieve certificate for a company
    /// </summary>
    /// <returns>Certificate</returns>
    /// <response code="200">License info</response>
    /// <response code="404">License not found</response>
    [HttpGet]
    [Produces(MediaTypeNames.Text.Plain)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCertificate(string company)
    {
        var certificate = $"{_certificateDirectory}\\{company.ToLower()}.txt";
        var fileBytes = System.IO.File.ReadAllBytes(certificate);
        return File(fileBytes, "text/plain", $"{company.ToLower()}.txt");
    }

    /// <summary>
    /// Retrieve certificate for a company
    /// </summary>
    /// <returns>Certificate</returns>
    /// <response code="200">License info</response>
    /// <response code="404">License not found</response>
    [HttpPut("/generateCertificate")]
    [Produces(MediaTypeNames.Text.Plain)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerateCertificate(
        [FromBody] GenerateCertificateDefinition certificate)
    {
        try
        {
            var license =
                await _licenseService.UpdateLicenseAsync(certificate.Company, certificate.ActualAmount);
            if (license != null) _certificateService.CreateCertificate(license);
            return Ok();
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
    }
}
