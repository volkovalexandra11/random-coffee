using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

namespace RandomCoffeeServer.Controllers.WebControllers;

[ApiController]
public class LoginController : ControllerBase
{
    public LoginController(
        SignInManager<IdentityCoffeeUser> signInManager,
        UserManager<IdentityCoffeeUser> userManager,
        IWebHostEnvironment environment,
        ILogger<LoginController> log)
    {
        this.environment = environment;
        this.log = log;
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    [HttpGet]
    [Route("login/google-login")]
    public async Task<IActionResult> Login()
    {
        log.LogInformation("User is trying to login with Google");
        var redirectUri = RedirectUri("/login/google-response");

        var properties = signInManager.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme, redirectUri);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet]
    [Route("login/google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        log.LogInformation("Got response from user to Google login");
        var loginInfo = await signInManager.GetExternalLoginInfoAsync();
        var email = loginInfo?.Principal.FindFirst(ClaimTypes.Email)?.Value;
        if (loginInfo == null)
            return Redirect(RedirectUri("/login"));

        var result = await signInManager.ExternalLoginSignInAsync(
            loginInfo.LoginProvider,
            loginInfo.ProviderKey,
            isPersistent: false);
        if (result.Succeeded)
        {
            log.LogInformation($"Successfully logged to existing account {email ?? "without email claim"}");
            return Redirect(RedirectUri("/"));
        }

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
        {
            log.LogError($"Error creating new user with email={user.Email}");
            return Unauthorized();
        }

        var loginCreationResult = await userManager.AddLoginAsync(user, loginInfo);
        if (!loginCreationResult.Succeeded)
        {
            log.LogError($"Error adding google credentials for new user with email={user.Email}");
            return Unauthorized();
        }

        await signInManager.SignInAsync(user, false);
        log.LogInformation($"Successfully created new user with email=${user.Email} and signed in");
        return Redirect(RedirectUri("/"));
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        log.LogInformation($"Logging out user with ${HttpContext.GetUserId()}");
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
    private readonly ILogger<LoginController> log;
    private readonly SignInManager<IdentityCoffeeUser> signInManager;
    private readonly UserManager<IdentityCoffeeUser> userManager;
    private const int DevFrontPort = 3000;
}