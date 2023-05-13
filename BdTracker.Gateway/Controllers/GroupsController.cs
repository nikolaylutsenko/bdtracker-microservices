using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BdTracker.Gateway.Dto.Requests;
using BdTracker.Gateway.Dto.Responses;
using BdTracker.Shared.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RestSharp;
using RestSharp.Authenticators;

namespace BdTracker.Gateway.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly RestClient _restClient;

    public GroupsController(IServiceAddressesSettings settings)
    {
        _restClient = new RestClient(settings.Groups);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups(CancellationToken token)
    {
        // Retrieve the bearer token from the Authorization header
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        _restClient.Authenticator = new JwtAuthenticator(bearerToken);
        var request = new RestRequest();

        var response = await _restClient.ExecuteGetAsync<IEnumerable<GroupResponse>>(request, token);
        if (!response.IsSuccessful)
            return BadRequest(response.ErrorMessage);

        return Ok(response.Content);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        // Retrieve the bearer token from the Authorization header
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        _restClient.Authenticator = new JwtAuthenticator(bearerToken);
        var request = new RestRequest($"/{id}");

        var response = await _restClient.ExecuteGetAsync<GroupResponse>(request, token);
        if (!response.IsSuccessful)
            return BadRequest(response.ErrorMessage);

        return Ok(response.Content);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup(CreateGroupRequest createGroupRequest, CancellationToken token)
    {
        // Retrieve the bearer token from the Authorization header
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        _restClient.Authenticator = new JwtAuthenticator(bearerToken);
        var request = new RestRequest().AddJsonBody(createGroupRequest);

        var response = await _restClient.ExecutePostAsync<GroupResponse>(request, token);
        if (!response.IsSuccessful)
            return BadRequest(response.ErrorMessage);

        return Ok(response.Content);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(Guid id, UpdateGroupRequest updateGroupRequest, CancellationToken token)
    {
        // Retrieve the bearer token from the Authorization header
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        _restClient.Authenticator = new JwtAuthenticator(bearerToken);
        var request = new RestRequest($"/{id}").AddJsonBody(updateGroupRequest);

        var response = await _restClient.ExecutePutAsync<GroupResponse>(request, token);
        if (!response.IsSuccessful)
            return BadRequest(response.ErrorMessage);

        return Ok(response.Content);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(Guid id, CancellationToken token)
    {
        // Retrieve the bearer token from the Authorization header
        var bearerToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        _restClient.Authenticator = new JwtAuthenticator(bearerToken);
        var request = new RestRequest($"/{id}", Method.Delete);

        var response = await _restClient.ExecuteAsync(request, token);
        if (!response.IsSuccessful)
            return BadRequest(response.ErrorMessage);

        return NoContent();
    }
}