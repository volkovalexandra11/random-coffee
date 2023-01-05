namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class Role
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
}