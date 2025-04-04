using System.Net.Mime;
using LicenseService.Application.Models;
using LicenseService.Domain.Exceptions;
using LicenseService.Domain.Interfaces;
using LicenseService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseService.Application.Controllers;

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
    /// log in a user
    /// </summary>
    /// <param name="loginDefinition"></param>
    /// <returns>User</returns>
    /// <response code="200">License update</response>
    /// <response code="400">Invalid data</response>
    /// <response code="406">Invalid Password</response>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> Login([FromBody] LoginDefinition loginDefinition)
    {
        try
        {
            return Ok(await _userService.Login(loginDefinition.Email, loginDefinition.Password));
        }
        catch (NotFoundException exception)
        {
            return Problem(statusCode: 404, detail: exception.Message);
        }
        catch (UnsuccessfulLoginException exception)
        {
            return Problem(statusCode: 406, detail: exception.Message);
        }
    }
}