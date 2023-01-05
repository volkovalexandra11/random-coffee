using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Dtos;

namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class CoffeeUserStore : IUserStore<User>
{
    public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));
        
        return user.UserId.ToString();
    }

    public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.UserId.ToString();
    }

    public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.UserName = userName;
    }

    public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.NormalizedUserName;
    }

    public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.NormalizedUserName = normalizedName;
    }
    
    public void Dispose()
    {
    }
}
