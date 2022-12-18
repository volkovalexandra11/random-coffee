using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

public class GroupDto
{
    public Guid GroupId { get; init; }
    public Guid AdminUserId { get; init; }
    public string Name { get; init; }
    public bool IsPrivate { get; init; }
    public List<string> Users { get; init; }

    
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

    public static GroupDto FromYdbRow(ResultSet.Row row, List<string> users)
    {
        return new GroupDto
        {
            GroupId = new Guid(row["group_id"].GetOptionalString()),
            AdminUserId = new Guid(row["admin_user_id"].GetOptionalString()),
            Name = (string)row["name"],
            IsPrivate = row["is_private"].GetOptionalInt32() != 0,
            Users = users
        };
    }
}