using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

public class IdentityUserModel
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? ProfilePictureUrl { get; init; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["user_id"] = YdbValue.MakeString(UserId.ToByteArray()),
            ["email"] = YdbValue.MakeUtf8(Email),
            ["first_name"] = YdbValue.MakeUtf8(FirstName),
            ["last_name"] = YdbValue.MakeUtf8(LastName),
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