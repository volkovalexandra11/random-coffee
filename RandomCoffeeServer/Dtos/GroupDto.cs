using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

public class GroupDto
{
    public Guid GroupId { get; init; }
    public Guid AdminUserId { get; init; }
    public string Name { get; init; }
    public bool IsPrivate { get; init; }

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
}