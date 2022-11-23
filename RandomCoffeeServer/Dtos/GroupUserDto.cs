using Ydb.Sdk.Value;

namespace RandomCoffee.Dtos;

public class GroupUserDto
{
    public ulong GroupId { get; init; }
    public ulong UserId { get; init; }
    
    public YdbValue ToYdb()
    {
        return YdbValue.MakeStruct(new Dictionary<string, YdbValue>
        {
            ["group_id"] = YdbValue.MakeUint64(GroupId),
            ["user_id"] = YdbValue.MakeUint64(UserId),
        });
    }
}