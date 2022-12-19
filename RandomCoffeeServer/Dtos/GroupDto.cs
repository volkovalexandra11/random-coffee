using RandomCoffeeServer.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

public class GroupDto
{
    public Guid GroupId { get; init; }
    public Guid AdminUserId { get; init; }
    public string Name { get; init; }
    public bool IsPrivate { get; init; } = true;
    
    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["group_id"] = YdbValue.MakeString(GroupId.ToByteArray()),
            ["admin_user_id"] = YdbValue.MakeString(AdminUserId.ToByteArray()),
            ["name"] = YdbValue.MakeUtf8(Name),
            ["is_private"] = YdbValue.MakeInt32(IsPrivate ? 1 : 0)
        };
    }

    public static GroupDto FromYdbRow(ResultSet.Row row)
    {
        return new GroupDto
        {
            GroupId = row["group_id"].GetGuid(),
            AdminUserId = row["admin_user_id"].GetGuid(),
            Name = row["name"].GetUtf8(),
            IsPrivate = row["is_private"].GetOptionalInt32() != 0,
        };
    }
}