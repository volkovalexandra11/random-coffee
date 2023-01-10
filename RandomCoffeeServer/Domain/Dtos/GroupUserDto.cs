using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Domain.Dtos;

public class GroupUserDto
{
    public Guid GroupId { get; init; }
    public Guid UserId { get; init; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["group_id"] = GroupId.ToYdb(),
            ["user_id"] = UserId.ToYdb(),
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