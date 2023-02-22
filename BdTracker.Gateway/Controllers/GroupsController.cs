using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace BdTracker.Gateway.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    public GroupsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost:7771/groups") // TODO: move it in separate file
        {
            Headers =
            {
                {HeaderNames.Accept, "application/json"},
                //{HeaderNames.UserAgent, "HttpRequestSample"}
            }
        };

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequest);

        httpResponseMessage.EnsureSuccessStatusCode();

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));

        var result = await JsonSerializer.DeserializeAsync<IEnumerable<GroupResponse>>(contentStream, options);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:7771/groups/{id}") // TODO: move it in separate file
        {
            Headers =
            {
                {HeaderNames.Accept, "application/json"},
                //{HeaderNames.UserAgent, "HttpRequestSample"}
            }
        };

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequest);

        httpResponseMessage.EnsureSuccessStatusCode();

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));

        var result = await JsonSerializer.DeserializeAsync<GroupResponse>(contentStream, options);

        return Ok(result);
    }
}

public record GroupResponse(Guid Id, string Name);
