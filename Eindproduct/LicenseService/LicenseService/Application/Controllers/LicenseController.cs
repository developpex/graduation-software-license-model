using System.Net.Mime;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseService.Application.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class LicenseController : ControllerBase
{
    private readonly ILicenseService _licenseService;
    private readonly ITokenHelper _tokenHelper;

    public LicenseController(ILicenseService licenseService, ITokenHelper tokenHelper)
    {
        _licenseService = licenseService;
        _tokenHelper = tokenHelper;
    }

    /// <summary>
    /// Retrieve all licenses
    /// </summary>
    /// <returns>License</returns>
    /// <response code="200">All licenses</response>
    /// <response code="404">Licenses not found</response>
    [HttpGet("/licenses")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLicensesAsync()
    {
        try
        {
            return Ok(await _licenseService.GetLicensesAsync());
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
    }

    /// <summary>
    /// Retrieve detailed information for the license status of a company
    /// </summary>
    /// <returns>License</returns>
    /// <response code="200">All licenses</response>
    /// <response code="404">Licenses not found</response>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(License))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLicenseAsync()
    {
        var company = _tokenHelper.GetClaimValue(GetBearerToken(Request), "company");

        try
        {
            return Ok(await _licenseService.GetLicenseAsync(company));
        }
        catch (NotFoundException exception )
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
    }

    /// <summary>
    /// Update a license
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>Updated license</returns>
    /// <response code="200">License update</response>
    /// <response code="400">Invalid data</response>
    /// <response code="404">Company not found</response>
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(License))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLicenseAsync(
        [FromBody] int amount)
    {
        var company = _tokenHelper.GetClaimValue(GetBearerToken(Request), "company");

        try
        {
            return Ok(await _licenseService.UpdateLicenseAsync(company, amount));
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
    }

    /// <summary>
    /// Update the last payment of a license
    /// </summary>
    /// <param name="company"></param>
    /// <returns>Updated license</returns>
    /// <response code="200">License update</response>
    /// <response code="400">Invalid data</response>
    /// <response code="404">Company not found</response>
    [HttpPut("/lastPayment")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(License))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLicensePaymentAsync([FromBody] string company)
    {
        try
        {
            return Ok(await _licenseService.UpdateLicensePaymentAsync(company));
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
    }

    private string GetBearerToken(HttpRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var header = request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(header))
        {
            return string.Empty;
        }

        var basicValue = header[header.IndexOf("Bearer", StringComparison.Ordinal)..];
        if (!basicValue.StartsWith("Bearer", StringComparison.InvariantCulture))
        {
            return string.Empty;
        }
            
        return basicValue["Bearer ".Length..].Trim();
    }
}