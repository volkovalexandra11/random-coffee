using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

public class IdentityUserLoginsOnlyStore : RepositoryBase
{
    private YdbTable UserLoginsAsp { get; }

    public IdentityUserLoginsOnlyStore(YdbService ydb) : base(ydb)
    {
        UserLoginsAsp = Schema.UserLoginsAsp;
    }

    public async Task AddLoginAsync(Guid userId, UserLoginInfo login)
    {
        // todo null-check

        var userLoginDto = new IdentityUserLogin()
        {
            LoginProvider = login.LoginProvider,
            ProviderKey = login.ProviderKey,
            ProviderDisplayName = login.ProviderDisplayName,
            UserId = userId
        };

        await UserLoginsAsp
            .Insert(userLoginDto.ToYdb())
            .ExecuteNonData(Ydb);
    }

    [Obsolete("for mock data only")]
    public async Task ReplaceLoginAsync(Guid userId, UserLoginInfo login)
    {
        var userLoginDto = new IdentityUserLogin()
        {
            LoginProvider = login.LoginProvider,
            ProviderKey = login.ProviderKey,
            ProviderDisplayName = login.ProviderDisplayName,
            UserId = userId
        };

        await UserLoginsAsp
            .Replace(userLoginDto.ToYdb())
            .ExecuteNonData(Ydb);
    }

    public async Task RemoveLoginAsync(string loginProvider, string providerKey)
    {
        await UserLoginsAsp
            .Delete()
            .Where("login_provider", loginProvider.ToYdb())
            .Where("provider_key", providerKey.ToYdb())
            .ExecuteNonData(Ydb);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(Guid userId)
    {
        var userLoginInfos = await UserLoginsAsp
            .Select()
            .ViewByColumn("user_login")
            .Where("user_id", userId.ToYdb())
            .ExecuteData(Ydb);


        return userLoginInfos.Select(IdentityUserLogin.FromYdbRow)
            .Select(dto => new UserLoginInfo(dto.LoginProvider, dto.ProviderKey, dto.ProviderDisplayName))
            .ToList();
    }

    public async Task<Guid?> FindUserIdByLoginAsync(string loginProvider, string providerKey)
    {
        var userIds = await UserLoginsAsp
            .Select("user_id")
            .Where("login_provider", loginProvider.ToYdb())
            .Where("provider_key", providerKey.ToYdb())
            .ExecuteData(Ydb);

        return userIds.SingleOrNoValue(row => row["user_id"].GetNonNullGuid());
    }
}