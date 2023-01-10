using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

public class IdentityUserInfoStore : RepositoryBase
{
    private YdbTable UserInfoAsp { get; }

    public IdentityUserInfoStore(YdbService ydb) : base(ydb)
    {
        UserInfoAsp = Schema.UserInfoAsp;
    }

    public async Task AddUserInfo(IdentityUserInfo identityUserInfo)
    {
        await UserInfoAsp
            .Insert(identityUserInfo.ToYdb()) // todo if not exists
            .ExecuteNonData(Ydb);
    }

    public async Task UpdateUserInfo(IdentityUserInfo identityUserInfo)
    {
        await UserInfoAsp
            .Replace(identityUserInfo.ToYdb()) // todo if exists
            .ExecuteNonData(Ydb);
    }

    [Obsolete("for mock data only")]
    public async Task ReplaceUserInfo(IdentityUserInfo identityUserInfo)
    {
        await UserInfoAsp
            .Replace(identityUserInfo.ToYdb()) // todo if exists
            .ExecuteNonData(Ydb);
    }

    public async Task<IdentityUserInfo?> FindUserInfo(Guid userId)
    {
        var userInfos = await UserInfoAsp
            .Select()
            .Where("user_id", userId.ToYdb())
            .ExecuteData(Ydb);

        return userInfos.SingleOrNull(userInfo => IdentityUserInfo.FromYdbRow(userInfo));
    }

    public async Task<IdentityUserInfo?> FindUserInfoByNormalizedUsername(string normalizedUsername)
    {
        var userInfos = await UserInfoAsp
            .Select()
            .ViewByColumn("normalized_username")
            .Where("normalized_username", normalizedUsername.ToYdb())
            .ExecuteData(Ydb);

        return userInfos.SingleOrNull(IdentityUserInfo.FromYdbRow);
    }

    public async Task DeleteUserInfo(Guid userId)
    {
        await UserInfoAsp
            .Delete()
            .Where("user_id", userId.ToYdb())
            .ExecuteNonData(Ydb);
    }
}