using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("loginin")]
public class LoginController : ControllerBase
{
    private readonly Random random = new ();
    [HttpGet]
    public async Task Login()
    {
        Console.WriteLine("HIIIIIIIIIIIII");
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme);
        // Console.WriteLine("hi");
        // var userId = Guid.Parse("43ef1000-0000-0000-0000-000000000000");
        // return Ok($"{{\"userId\": \"{userId}\"}}");
    }
}

[ApiController]
[Route("logout")]
public class LogoutController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);
        return Redirect("/login");
        // Console.WriteLine("hi");
        // var userId = Guid.Parse("43ef1000-0000-0000-0000-000000000000");
        // return Ok($"{{\"userId\": \"{userId}\"}}");
    }
}

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        return Ok("Teeest");
        // Console.WriteLine("hi");
        // var userId = Guid.Parse("43ef1000-0000-0000-0000-000000000000");
        // return Ok($"{{\"userId\": \"{userId}\"}}");
    }
}