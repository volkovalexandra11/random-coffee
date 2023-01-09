using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

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

    public async Task RemoveLoginAsync(string loginProvider, string providerKey)
    {
        await UserLoginsAsp
            .Delete()
            .Where("login_provider", YdbValue.MakeUtf8(loginProvider))
            .Where("provider_key", YdbValue.MakeUtf8(providerKey))
            .ExecuteNonData(Ydb);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(Guid userId)
    {
        var userLoginInfos = await UserLoginsAsp
            .Select()
            .ViewByColumn("user_login")
            .Where("user_id", YdbValue.MakeString(userId.ToByteArray()))
            .ExecuteData(Ydb);
        
        
        return userLoginInfos.Select(IdentityUserLogin.FromYdbRow)
            .Select(dto => new UserLoginInfo(dto.LoginProvider, dto.ProviderKey, dto.ProviderDisplayName))
            .ToList();
    }

    public async Task<Guid?> FindUserIdByLoginAsync(string loginProvider, string providerKey)
    {
        var userIds = await UserLoginsAsp
            .Select("user_id")
            .Where("login_provider", YdbValue.MakeUtf8(loginProvider))
            .Where("provider_key", YdbValue.MakeUtf8(providerKey))
            .ExecuteData(Ydb);

        return userIds.SingleOrDefault(row => row["user_id"].GetNonNullGuid());
    }
}