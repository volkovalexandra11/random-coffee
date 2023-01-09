using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

namespace RandomCoffeeServer.Controllers.ApiControllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    public AccountController(UserManager<IdentityCoffeeUser> userManager)
    {
        this.userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetLoginInfo()
    {
        var userId = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        return Ok(await userManager.FindByIdAsync(userId));
    }

    private readonly UserManager<IdentityCoffeeUser> userManager;
}