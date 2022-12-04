using Ydb.Sdk.Value;

namespace RandomCoffee.Dtos;

public class GroupUserDto
{
    public Guid GroupId { get; init; }
    public Guid UserId { get; init; }
    
    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["group_id"] = YdbValue.MakeString(GroupId.ToByteArray()),
            ["user_id"] = YdbValue.MakeString(UserId.ToByteArray()),
        };
    }
}