using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

namespace RandomCoffeeServer.Controllers.WebControllers;

[ApiController]
public class LoginController : ControllerBase
{
    public LoginController(SignInManager<IdentityCoffeeUser> signInManager, UserManager<IdentityCoffeeUser> userManager,
        IWebHostEnvironment environment)
    {
        this.environment = environment;
        this.signInManager = signInManager;
        this.userManager = userManager;
    }
    
    [HttpGet]
    [Route("login/google-login")]
    public async Task<IActionResult> Login()
    {
        var redirectUri = RedirectUri("/login/google-response");

        var properties = signInManager.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme, redirectUri);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [Route("login/google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var loginInfo = await signInManager.GetExternalLoginInfoAsync();
        if (loginInfo == null)
            return Redirect(RedirectUri("/login"));

        var result = await signInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false);
        if (result.Succeeded)
            return Redirect(RedirectUri("/"));
        
        var email = loginInfo.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var user = new IdentityCoffeeUser()
        {
            UserId = Guid.NewGuid(), // todo не тут
            FirstName = loginInfo.Principal.FindFirst(ClaimTypes.GivenName)?.Value,
            LastName = loginInfo.Principal.FindFirst(ClaimTypes.Surname)?.Value,
            Email = email,
            ProfilePictureUrl = loginInfo.Principal.FindFirst("image")?.Value,
            
            UserName = email,
            NormalizedUserName = email.ToUpperInvariant()
        };

        var userCreationResult = await userManager.CreateAsync(user);
        if (!userCreationResult.Succeeded)
            return Unauthorized();

        var loginCreationResult = await userManager.AddLoginAsync(user, loginInfo);
        if (!loginCreationResult.Succeeded)
            return Unauthorized();

        await signInManager.SignInAsync(user, false);
        return Redirect(RedirectUri("/"));
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Redirect(RedirectUri("/"));
    }

    private string RedirectUri(string route)
    {
        return environment.IsDevelopment()
            ? $"http://localhost:{DevFrontPort}{route}"
            : route;
    }

    private readonly IWebHostEnvironment environment;
    private readonly SignInManager<IdentityCoffeeUser> signInManager;
    private readonly UserManager<IdentityCoffeeUser> userManager;
    private const int DevFrontPort = 3000;
}