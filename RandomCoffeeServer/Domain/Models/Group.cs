using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Domain.Models;

public class Group
{
    public Guid GroupId { get; init; }
    public Guid AdminUserId { get; init; }
    public string Name { get; init; }
    public string Tag { get; init; }
    public bool IsPrivate { get; init; } = true;
    public string? GroupPictureUrl { get; init; }

    public Dictionary<string, YdbValue> ToYdb() //TODO(Cockamamie): все имена столбцов в класс с константами
    {
        return new Dictionary<string, YdbValue>
        {
            ["group_id"] = GroupId.ToYdb(),
            ["admin_user_id"] = AdminUserId.ToYdb(),
            ["name"] = Name.ToYdb(),
            ["tag"] = Tag.ToYdb(),
            ["is_private"] = YdbValue.MakeInt32(IsPrivate ? 1 : 0),
            ["group_picture_url"] = YdbValue.MakeUtf8(GroupPictureUrl ?? "")
        };
    }

    public static Group FromYdbRow(ResultSet.Row row)
    {
        return new Group
        {
            GroupId = row["group_id"].GetNonNullGuid(),
            AdminUserId = row["admin_user_id"].GetNonNullGuid(),
            Name = row["name"].GetOptionalUtf8(),
            Tag = row["tag"].GetOptionalUtf8(),
            IsPrivate = row["is_private"].GetNonNullBool(),
            GroupPictureUrl = row["group_picture_url"].GetOptionalUtf8()
        };
    }
}