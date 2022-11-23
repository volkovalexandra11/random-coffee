using RandomCoffee.Dtos;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee;

public class GroupService
{
    private readonly YdbService ydbService;

    public GroupService(YdbService ydbService)
    {
        this.ydbService = ydbService;
    }

    public async Task AddGroup(GroupDto group)
    {
        var groups = Schema.Tables.Single(t => t.TableName == "groups");
        var declare = QueryBuilder.QueryBuilder.AsTableDeclare(groups);
        await ydbService.Execute($"{declare};\nUPSERT INTO groups SELECT * FROM AS_TABLE($data);", new Dictionary<string, YdbValue>()
        {
            { "data", group.ToYdb() }
        });
    }

    public async Task AddToGroup(GroupUserDto groupUser)
    {
        var groupsUsers = Schema.Tables.Single(t => t.TableName == "groups_users");
        var declare = QueryBuilder.QueryBuilder.AsTableDeclare(groupsUsers);
        await ydbService.Execute($"{declare};\nUPSERT INTO groups_users SELECT * FROM AS_TABLE($data);", new Dictionary<string, YdbValue>()
        {
            { "data", groupUser.ToYdb() }
        });
    }
}