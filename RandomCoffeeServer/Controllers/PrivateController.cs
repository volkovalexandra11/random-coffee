using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RandomCoffeeServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PrivateController : ControllerBase
{
    public PrivateController()
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var token = await HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, "access_token");
        Console.WriteLine(token);
        
        return Ok("Hello, world!");
    }
}