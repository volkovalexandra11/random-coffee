using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

public class GroupRepository : RepositoryBase
{
    public GroupRepository(YdbService ydb)
        : base(ydb, "groups")
    {
    }

    public async Task AddGroup(Group group)
    {
        var @params = YdbConverter.ToDataParams(group.ToYdb());
        await Ydb.Execute(
            $"{DeclareStatement}\n" +
            $"REPLACE INTO groups SELECT * FROM AS_TABLE($data);",
            @params);
    }

    public async Task<Group?> FindGroup(Guid groupId)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT * FROM groups WHERE group_id = $id;",
            new Dictionary<string, YdbValue>
            {
                ["$id"] = YdbValue.MakeString(groupId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? Group.FromYdbRow(resultSet.Rows[0]) : null;
    }
}