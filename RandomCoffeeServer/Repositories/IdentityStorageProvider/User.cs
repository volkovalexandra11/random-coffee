namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }

    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
}