using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

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
            ["login_provider"] = LoginProvider.ToYdb(),
            ["provider_key"] = ProviderKey.ToYdb(),
            ["provider_display_name"] = ProviderDisplayName.ToYdb(),
            ["user_id"] = UserId.ToYdb()
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