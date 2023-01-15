using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        var user = await HttpContext.GetUserAsync(userManager);
        if (user is null)
            throw new InvalidProgramException();

        return Ok(user);
    }

    private readonly UserManager<IdentityCoffeeUser> userManager;
}