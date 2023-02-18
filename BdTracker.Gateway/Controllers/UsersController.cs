using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BdTracker.Gateway.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    public UsersController()
    {
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok();
    }

}