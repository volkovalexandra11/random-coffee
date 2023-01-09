using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

public class Role
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["role_id"] = YdbValue.MakeString(RoleId.ToByteArray()),
            ["name"] = YdbValue.MakeUtf8(Name),
            ["normalized_name"] = YdbValue.MakeUtf8(NormalizedName)
        };
    }

    public static Role FromYdbRow(ResultSet.Row row)
    {
        return new Role
        {
            RoleId = row["role_id"].GetNonNullGuid(),
            Name = row["name"].GetNonNullUtf8(),
            NormalizedName = row["normalized_name"].GetNonNullUtf8()
        };
    }
}