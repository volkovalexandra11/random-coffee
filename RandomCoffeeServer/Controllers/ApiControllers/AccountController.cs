using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Controllers.ApiControllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    public AccountController(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetLoginInfo()
    {
        var userId = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        return Ok(await userManager.FindByIdAsync(userId));
    }

    private readonly UserManager<User> userManager;
}