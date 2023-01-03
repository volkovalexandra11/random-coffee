using Microsoft.AspNetCore.Mvc;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("/login")]
public class LoginController : ControllerBase
{
    private readonly Random random = new ();
    [HttpPost]
    public async Task<IActionResult> Login()
    {
        var userId = Guid.Parse("43ef1000-0000-0000-0000-000000000000");
        return Ok($"{{\"userId\": \"{userId}\"}}");
    }
}