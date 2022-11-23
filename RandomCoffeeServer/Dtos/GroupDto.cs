using Ydb;
using Ydb.Sdk.Value;

namespace RandomCoffee.Dtos;

public class GroupDto
{
    public ulong GroupId { get; init; }
    public ulong AdminUserId { get; init; }
    public string Name { get; init; }
    public bool IsPrivate { get; init; }

    public YdbValue ToYdb()
    {
        return YdbValue.MakeStruct(new Dictionary<string, YdbValue>
        {
            ["group_id"] = YdbValue.MakeUint64(GroupId),
            ["admin_user_id"] = YdbValue.MakeUint64(AdminUserId),
            ["name"] = YdbValue.MakeUtf8(Name),
            ["IsPrivate"] = YdbValue.MakeInt32(IsPrivate ? 1 : 0)
        });
    }
}