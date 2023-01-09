using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

public class IdentityUserInfo
{
    public Guid UserId { get; set; }

    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["user_id"] = YdbValue.MakeString(UserId.ToByteArray()),
            ["username"] = YdbValue.MakeUtf8(UserName),
            ["normalized_username"] = YdbValue.MakeUtf8(NormalizedUserName)
        };
    }

    public static IdentityUserInfo FromYdbRow(ResultSet.Row row)
    {
        return new IdentityUserInfo
        {
            UserId = row["user_id"].GetNonNullGuid(),
            UserName = row["username"].GetNonNullUtf8(),
            NormalizedUserName = row["normalized_username"].GetNonNullUtf8()
        };
    }
}