using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

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

    // Guid[] if found some users, null if no users found (<=> group doesn't exist) 
    public async Task<Guid[]?> FindUsersInGroup(Guid groupId)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT user_id FROM groups_users WHERE group_id = $id;",
            new Dictionary<string, YdbValue>
            {
                ["$id"] = YdbValue.MakeString(groupId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count > 0
            ? resultSet.Rows.Select(row => row["user_id"].GetNonNullGuid()).ToArray()
            : null;
    }

    public async Task<Guid[]> FindGroupsByUser(Guid userId)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT group_id " +
            $"FROM groups_users VIEW groups_users_by_user_id " +
            $"WHERE user_id = $id;", new Dictionary<string, YdbValue>
            {
                ["$id"] = YdbValue.MakeString(userId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Select(row => row["group_id"].GetNonNullGuid()).ToArray();
    }
}