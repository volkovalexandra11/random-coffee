using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

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

    public async Task<UserDto?> FindUser(Guid userId)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT * FROM users WHERE user_id = $id;",
            new Dictionary<string, YdbValue>
            {
                ["$id"] = YdbValue.MakeString(userId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1
            ? UserDto.FromYdbRow(resultSet.Rows[0])
            : null;
    }
}