using RandomCoffee.Dtos;
using RandomCoffee.QueryBuilder;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee.Repositories;

public class GroupUserRepository : RepositoryBase
{
    public GroupUserRepository(YdbService ydb)
        : base(ydb, "groups_users")
    {
    }

    public async Task AddToGroup(GroupUserDto groupUser)
    {
        var @params = YdbValueConverter.ToDataParams(groupUser.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement};\n" +
            $"UPSERT INTO groups_users SELECT * FROM AS_TABLE($data);",
            @params);
    }
}