using Microsoft.AspNetCore.Identity;

namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class CoffeeRoleStore : IRoleStore<Role>
{
    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        return role.RoleId.ToString();
    }

    public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        return role.Name;
    }

    public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        role.Name = roleName;
    }

    public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        return role.NormalizedName;
    }

    public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        role.NormalizedName = normalizedName;
    }

    public void Dispose()
    {
    }
}