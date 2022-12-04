using Microsoft.AspNetCore.Mvc;

namespace RandomCoffee.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    // public TestController(YdbService db) : base(db)
    // {
    // }

    [HttpGet("")]
    public IActionResult Test()
    {
        return Ok("hi");
    }
}