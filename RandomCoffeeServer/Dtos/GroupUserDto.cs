using RandomCoffeeServer.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

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

    public static GroupUserDto FromYdbRow(ResultSet.Row row)
    {
        return new GroupUserDto
        {
            GroupId = row["group_id"].GetNonNullGuid(),
            UserId = row["user_id"].GetNonNullGuid()
        };
    }
}