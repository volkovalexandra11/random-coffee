using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

public class IdentityUserStore : IUserLoginStore<IdentityCoffeeUser>
{
    private readonly UserRepository userRepository;
    private readonly IdentityUserInfoStore userInfoStore;
    private readonly IdentityUserLoginsOnlyStore userLoginsStore;

    public IdentityUserStore(
        UserRepository userRepository,
        IdentityUserInfoStore userInfoStore,
        IdentityUserLoginsOnlyStore userLoginsStore)
    {
        this.userRepository = userRepository;
        this.userInfoStore = userInfoStore;
        this.userLoginsStore = userLoginsStore;
    }

    public async Task AddLoginAsync(IdentityCoffeeUser user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        await userLoginsStore.AddLoginAsync(user.UserId, login);
    }

    public async Task RemoveLoginAsync(IdentityCoffeeUser user, string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        await userLoginsStore.RemoveLoginAsync(loginProvider, providerKey);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        return await userLoginsStore.GetLoginsAsync(user.UserId);
    }

    public async Task<IdentityCoffeeUser?> FindByLoginAsync(string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        var userId = await userLoginsStore.FindUserIdByLoginAsync(loginProvider, providerKey);
        if (userId is null)
            return null;

        var user = await userRepository.FindUser(userId.Value);
        if (user is null)
            throw new InvalidProgramException();

        return await EnrichFromUserModel(user);
    }

    public async Task<IdentityResult> CreateAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        // todo null-check      
        try
        {
            await userRepository.AddUser(user.ToModel());
            await userInfoStore.AddUserInfo(user.ToIdentityInfo());
        }
        catch (Exception e)
        {
            return IdentityResult.Failed();
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        try
        {
            await userRepository.UpdateUser(user.ToModel()); // todo транзакцией
            await userInfoStore.UpdateUserInfo(user.ToIdentityInfo());
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed();
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        try
        {
            await userRepository.DeleteUser(user.UserId); // todo транзакцией
            await userInfoStore.DeleteUserInfo(user.UserId);
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(); // todo what if deleted but err on confirmation?
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityCoffeeUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUser(Guid.Parse(userId));
        if (user is null)
            return null;

        return await EnrichFromUserModel(user);
    }

    public async Task<IdentityCoffeeUser?> FindByNameAsync(string normalizedUserName,
        CancellationToken cancellationToken)
    {
        var userInfo = await userInfoStore.FindUserInfoByNormalizedUsername(normalizedUserName);
        if (userInfo is null)
            return null;
        
        var user = await userRepository.FindUser(userInfo.UserId);
        if (user is null)
            throw new InvalidProgramException();

        return new IdentityCoffeeUser
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            UserName = userInfo.UserName,
            NormalizedUserName = userInfo.NormalizedUserName
        };
    }

    public async Task<string> GetUserIdAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.UserId.ToString();
    }

    public async Task<string> GetUserNameAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.UserName;
    }

    public async Task SetUserNameAsync(IdentityCoffeeUser user, string userName, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.UserName = userName;
    }

    public async Task<string> GetNormalizedUserNameAsync(IdentityCoffeeUser user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.NormalizedUserName;
    }

    public async Task SetNormalizedUserNameAsync(IdentityCoffeeUser user, string normalizedName,
        CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.NormalizedUserName = normalizedName;
    }

    public void Dispose()
    {
    }

    private async Task<IdentityCoffeeUser?> EnrichFromUserModel(User user)
    {
        var userInfo = await userInfoStore.FindUserInfo(user.UserId);
        if (userInfo is null)
            throw new InvalidProgramException();

        return new IdentityCoffeeUser
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePictureUrl = user.ProfilePictureUrl,
            UserName = userInfo.UserName,
            NormalizedUserName = userInfo.NormalizedUserName
        };
    }
}