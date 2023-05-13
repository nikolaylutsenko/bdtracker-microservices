using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BdTracker.Auth.Dto.Requests;
using BdTracker.Auth.Dto.Responses;
using MapsterMapper;
using BdTracker.Shared.Settings;

namespace BdTracker.Auth.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LoginController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<LoginController> _logger;
    private readonly IAuthSettings _authSettings;
    private readonly IMapper _mapper;

    public LoginController(UserManager<IdentityUser> userManager, ILogger<LoginController> logger, IMapper mapper, IAuthSettings authSettings)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _authSettings = authSettings;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync(LoginRequest request)
    {
        var identityUsr = await _userManager.FindByEmailAsync(request.Email);

        if (identityUsr is null)
        {
            return BadRequest();
        }

        var userRole = await _userManager.GetRolesAsync(identityUsr);

        if (await _userManager.CheckPasswordAsync(identityUsr, request.Password))
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _authSettings.Issuer,
                audience: _authSettings.Audience,
                new List<Claim>
                {
                    new Claim("Id", identityUsr.Id.ToString()),
                    new Claim(ClaimTypes.Role, userRole.First().ToString())
                },
                signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            var loginResponse = _mapper.Map<LoginResponse>(identityUsr);
            loginResponse = loginResponse with { Token = stringToken };

            return Ok(loginResponse);
        }
        else
        {
            return Unauthorized();
        }
    }
}