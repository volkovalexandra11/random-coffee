using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

public class IdentityRoleModel
{
    public Guid RoleId { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }

    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["role_id"] = RoleId.ToYdb(),
            ["name"] = Name.ToYdb(),
            ["normalized_name"] = NormalizedName.ToYdb()
        };
    }

    public static IdentityRoleModel FromYdbRow(ResultSet.Row row)
    {
        return new IdentityRoleModel
        {
            RoleId = row["role_id"].GetNonNullGuid(),
            Name = row["name"].GetNonNullUtf8(),
            NormalizedName = row["normalized_name"].GetNonNullUtf8()
        };
    }
}