using System.Net.Mime;
using LicenseManager.Application.Models;
using LicenseManager.Domain.Exceptions;
using LicenseManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LicenseManager.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Log the user into StockControl demo app
    /// </summary>
    /// <returns>User</returns>
    /// <response code="200">Logged in</response>
    /// <response code="404">User not found</response>
    /// <response code="406">Exceeding license amount</response>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login([FromBody] LoginDefinition loginDefinition)
    {
        try
        {
            return Ok(await _userService.Login(loginDefinition.Email, loginDefinition.Password));
        }
        catch (UnsuccessfulLoginException exception)
        {
            return Problem(statusCode: 401, detail: exception.Message);
        }
        catch (ExceedLicenseAmountException exception)
        {
            return Problem(statusCode: 403, detail: exception.Message);
        }
        catch (ExpiryDateException exception)
        {
            return Problem(statusCode: 409, detail: exception.Message);
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
    }
}
