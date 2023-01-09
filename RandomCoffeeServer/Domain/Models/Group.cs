using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Domain.Models;

public class Group
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

    public static Group FromYdbRow(ResultSet.Row row)
    {
        return new Group
        {
            GroupId = row["group_id"].GetNonNullGuid(),
            AdminUserId = row["admin_user_id"].GetNonNullGuid(),
            Name = row["name"].GetNonNullUtf8(),
            IsPrivate = row["is_private"].GetNonNullBool(),
        };
    }
}