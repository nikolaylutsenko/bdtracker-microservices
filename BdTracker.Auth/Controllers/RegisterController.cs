using System.Net.Mime;
using BdTracker.Auth.Dto.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BdTracker.Auth.Controllers;

[ApiController]
[Route("api/v1/registration")]
[Produces(MediaTypeNames.Application.Json)]
public class RegisterController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<RegisterController> _logger;

    public RegisterController(UserManager<IdentityUser> userManager, ILogger<RegisterController> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registration(RegistrationRequest request)
    {
        var id = Guid.NewGuid();
        var user = new IdentityUser
        {
            Id = id.ToString(),
            UserName = request.Email,
            NormalizedUserName = request.Email.ToUpper(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToUpper()
        };

        var addUserResult = await _userManager.CreateAsync(user, request.Password);

        if (addUserResult.Succeeded)
        {
            var setRoleResult = await _userManager.AddToRoleAsync(user, "User");

            if (setRoleResult.Succeeded)
            {
                return Ok(new { user.Id, user.Email });
            }

            return Problem();
        }
        else
        {
            return Problem();
        }
    }
}