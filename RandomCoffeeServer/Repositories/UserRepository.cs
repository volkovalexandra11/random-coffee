using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;

namespace RandomCoffeeServer.Repositories;

public class UserRepository : RepositoryBase
{
    public UserRepository(YdbService ydb)
        : base(ydb, "users")
    {
    }

    public async Task AddUser(UserDto user)
    {
        var @params = YdbConverter.ToDataParams(user.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            "REPLACE INTO users SELECT * FROM AS_TABLE($data);",
            @params
        );
    }
}