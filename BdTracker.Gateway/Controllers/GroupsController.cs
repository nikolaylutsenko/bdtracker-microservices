using System;
using System.Collections.Generic;
using System.Linq;
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
    public IActionResult GetAllGroups()
    {
        var httpRequest = new HttpRequestMessage(
            HttpMethod.Get,
            "here must be an address")
        {
            Headers =
            {
                {HeaderNames.Accept, "application/json"},
                {HeaderNames.UserAgent, "HttpRequestSample"}
            }
        };

        var httpClient = _httpClientFactory.CreateClient();

        return Ok();
    }
}