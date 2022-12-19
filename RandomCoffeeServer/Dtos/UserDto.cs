using RandomCoffeeServer.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Dtos;

public class UserDto
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

    public static UserDto FromYdbRow(ResultSet.Row row)
    {
        return new UserDto
        {
            UserId = row["user_id"].GetGuid(),
            FirstName = row["first_name"].GetUtf8(),
            LastName = row["last_name"].GetUtf8(),
            Email = row["email"].GetUtf8(),
            ProfilePictureUrl = row["profile_picture_url"].GetOptionalUtf8()
        };
    }
}