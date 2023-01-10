using RandomCoffeeServer.Domain.Models;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

public class IdentityCoffeeUser
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? ProfilePictureUrl { get; init; }

    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }

    public User ToModel()
    {
        return new User
        {
            UserId = UserId,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,
            ProfilePictureUrl = ProfilePictureUrl
        };
    }

    public IdentityUserInfo ToIdentityInfo()
    {
        return new IdentityUserInfo
        {
            UserId = UserId,
            UserName = UserName,
            NormalizedUserName = NormalizedUserName
        };
    }
}