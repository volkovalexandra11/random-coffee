using RandomCoffee.Dtos;
using RandomCoffee.QueryBuilder;

namespace RandomCoffee.Repositories;

public class GroupRepository : RepositoryBase
{
    public GroupRepository(YdbService ydb)
        : base(ydb, "groups")
    {
    }

    public async Task AddGroup(GroupDto group)
    {
        var @params = YdbValueConverter.ToDataParams(group.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            $"REPLACE INTO groups SELECT * FROM AS_TABLE($data);",
            @params);
    }
}