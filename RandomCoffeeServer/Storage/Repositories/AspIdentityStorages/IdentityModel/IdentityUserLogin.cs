using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Yandex.Cloud.Dataproc.V1;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

public class IdentityUserLogin
{
    public string LoginProvider { get; set; }
    public string ProviderKey { get; set; }
    public string ProviderDisplayName { get; set; }
    public Guid UserId { get; set; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["login_provider"] = YdbValue.MakeUtf8(LoginProvider),
            ["provider_key"] = YdbValue.MakeUtf8(ProviderKey),
            ["provider_display_name"] = YdbValue.MakeUtf8(ProviderDisplayName),
            ["user_id"] = YdbValue.MakeString(UserId.ToByteArray())
        };
    }

    public static IdentityUserLogin FromYdbRow(ResultSet.Row row)
    {
        return new IdentityUserLogin
        {
            LoginProvider = row["login_provider"].GetNonNullUtf8(),
            ProviderKey = row["provider_key"].GetNonNullUtf8(),
            ProviderDisplayName = row["provider_display_name"].GetNonNullUtf8(),
            UserId = row["user_id"].GetNonNullGuid()
        };
    }
}