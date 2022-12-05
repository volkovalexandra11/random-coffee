using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;

namespace RandomCoffeeServer.Repositories;

public class GroupUserRepository : RepositoryBase
{
    public GroupUserRepository(YdbService ydb)
        : base(ydb, "groups_users")
    {
    }

    public async Task AddToGroup(GroupUserDto groupUser)
    {
        var @params = YdbConverter.ToDataParams(groupUser.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            $"REPLACE INTO groups_users SELECT * FROM AS_TABLE($data);",
            @params);
    }
}