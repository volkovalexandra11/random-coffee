using RandomCoffeeServer.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }

    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["user_id"] = YdbValue.MakeString(UserId.ToByteArray()),
            ["email"] = YdbValue.MakeUtf8(Email),
            ["first_name"] = YdbValue.MakeUtf8(FirstName),
            ["last_name"] = YdbValue.MakeUtf8(LastName),
            ["profile_picture_url"] = YdbValue.MakeUtf8(ProfilePictureUrl ?? ""),
            ["username"] = YdbValue.MakeUtf8(UserName),
            ["normalized_username"] = YdbValue.MakeUtf8(NormalizedUserName)
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
            ProfilePictureUrl = row["profile_picture_url"].GetOptionalUtf8(),
            
            UserName = row["username"].GetNonNullUtf8(),
            NormalizedUserName = row["normalized_username"].GetNonNullUtf8()
        };
    }
}