using AutoMapper;
using BdTracker.Back.Controllers;
using BdTracker.Back.Services.Interfaces;
using BdTracker.Users.Dtos;
using BdTracker.Users.Dtos.Requests;
using BdTracker.Users.Dtos.Responses;
using BdTracker.Users.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BdTracker.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IService<User> _userService;

    public UsersController(ILogger<UsersController> logger, IMapper mapper, IService<User> userService) : base(logger, mapper)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();

        var result = _mapper.Map<IEnumerable<UserResponse>>(users);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var user = await _userService.GetAsync(id);

        if (user == null)
        {
            return NotFound($"User with id: [{id}] not found");
        }

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);

        var result = await _userService.AddAsync(user);

        return CreatedAtAction("Get", new { id = result.Id }, _mapper.Map<UserResponse>(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _userService.GetAsync(id);

        if (user == null)
        {
            return BadRequest($"User with id: [{id}] not found");
        }

        _mapper.Map(request, user);

        await _userService.UpdateAsync(user);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}