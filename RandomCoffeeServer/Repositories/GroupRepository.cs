using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;

namespace RandomCoffeeServer.Repositories;

public class GroupRepository : RepositoryBase
{
    public GroupRepository(YdbService ydb)
        : base(ydb, "groups")
    {
    }

    public async Task AddGroup(GroupDto group)
    {
        var @params = YdbConverter.ToDataParams(group.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            $"REPLACE INTO groups SELECT * FROM AS_TABLE($data);",
            @params);
    }
}