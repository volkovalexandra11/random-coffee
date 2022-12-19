using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

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

    public async Task<GroupDto?> FindGroup(Guid groupId)
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
        return resultSet.Rows.Count == 1 ? GroupDto.FromYdbRow(resultSet.Rows[0]) : null;
    }
}