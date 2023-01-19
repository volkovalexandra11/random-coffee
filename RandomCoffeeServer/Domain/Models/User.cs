using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Domain.Models;

public class User
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? ProfilePictureUrl { get; init; }

    public string FullName => $"{FirstName} {LastName}";

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["user_id"] = UserId.ToYdb(),
            ["email"] = Email.ToYdb(),
            ["first_name"] = FirstName.ToYdb(),
            ["last_name"] = LastName.ToYdb(),
            ["profile_picture_url"] = YdbValue.MakeUtf8(ProfilePictureUrl ?? "")
        };
    }

    public static User FromYdbRow(ResultSet.Row row)
    {
        return new User
        {
            UserId = row["user_id"].GetNonNullGuid(),
            FirstName = row["first_name"].GetNonNullUtf8(),
            LastName = row["last_name"].GetNonNullUtf8(),
            Email = row["email"].GetNonNullUtf8(),
            ProfilePictureUrl = row["profile_picture_url"].GetOptionalUtf8()
        };
    }
}