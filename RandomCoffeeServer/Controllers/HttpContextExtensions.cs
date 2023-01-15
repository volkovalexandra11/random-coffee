using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

namespace RandomCoffeeServer.Controllers;

public static class HttpContextExtensions
{
    public static Guid? GetUserId(this HttpContext httpContext)
    {
        var userIdString = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        if (userIdString is null)
            return null;

        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidProgramException("User id wasn't in Guid format");

        return userId;
    }

    public static async Task<User?> GetUserAsync(
        this HttpContext httpContext,
        UserManager<IdentityCoffeeUser> userManager)
    {
        var userId = httpContext.GetUserId();
        if (userId is null)
            return null;

        var identityUser = await userManager.FindByIdAsync(userId.Value.ToString());
        if (identityUser is null)
            throw new InvalidProgramException($"Found no user for authenticated id {userId}");
        return new User
        {
            UserId = identityUser.UserId,
            Email = identityUser.Email,
            FirstName = identityUser.FirstName,
            LastName = identityUser.LastName,
            ProfilePictureUrl = identityUser.ProfilePictureUrl
        };
    }
}