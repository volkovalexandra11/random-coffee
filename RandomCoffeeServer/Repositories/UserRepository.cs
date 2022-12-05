using RandomCoffee.Dtos;
using RandomCoffee.QueryBuilder;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee.Repositories;

public class UserRepository : RepositoryBase
{
    public UserRepository(YdbService ydb)
        : base(ydb, "users")
    {
    }

    public async Task AddUser(UserDto user)
    {
        var @params = YdbValueConverter.ToDataParams(user.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            "REPLACE INTO users SELECT * FROM AS_TABLE($data);",
            @params
        );
    }
}