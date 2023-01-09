using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

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

    public async Task<IdentityUserInfo?> FindUserInfo(Guid userId)
    {
        var userInfos = await UserInfoAsp
            .Select()
            .Where("user_id", YdbValue.MakeString(userId.ToByteArray()))
            .ExecuteData(Ydb);

        return userInfos.SingleOrDefault(userInfo => IdentityUserInfo.FromYdbRow(userInfo));
    }

    public async Task<IdentityUserInfo?> FindUserInfoByNormalizedUsername(string normalizedUsername)
    {
        var userInfos = await UserInfoAsp
            .Select()
            .ViewByColumn("normalized_username")
            .Where("normalized_username", YdbValue.MakeUtf8(normalizedUsername))
            .ExecuteData(Ydb);

        return userInfos.SingleOrDefault(IdentityUserInfo.FromYdbRow);
    }

    public async Task DeleteUserInfo(Guid userId)
    {
        await UserInfoAsp
            .Delete()
            .Where("user_id", YdbValue.MakeString(userId.ToByteArray()))
            .ExecuteNonData(Ydb);
    }
}