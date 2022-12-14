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
    
    public async Task<ResultSet> GetUsers(Guid id)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n"+ 
            $"SELECT * FROM groups_users WHERE group_id = $id;", new Dictionary<string, YdbValue>
            {
                {
                    "$id", YdbValue.MakeString(id.ToByteArray())
                }
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet;
    }
    
    public async Task<ResultSet> GetGroups(Guid id)
    {
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n"+ 
            $"SELECT * FROM groups_users WHERE user_id = $id;", new Dictionary<string, YdbValue>
            {
                {
                    "$id", YdbValue.MakeString(id.ToByteArray())
                }
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet;
    }
}