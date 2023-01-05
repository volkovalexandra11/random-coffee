using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class CoffeeRoleStore : RepositoryBase, IRoleStore<Role>
{
    public CoffeeRoleStore(YdbService ydb) : base(ydb, "roles_asp")
    {
    }

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
    {
        // todo null-check        

        var @params = YdbConverter.ToDataParams(role.ToYdb());
        try
        {
            await Ydb.Execute(
                $"{DeclareStatement}\n" +
                "INSERT INTO roles_asp SELECT * FROM AS_TABLE($data);",
                @params
            );
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed();
        }
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var @params = YdbConverter.ToDataParams(role.ToYdb());
        try
        {
            await Ydb.Execute(
                $"{DeclareStatement}\n" +
                "REPLACE INTO roles_asp SELECT * FROM AS_TABLE($data);", // todo UPDATE?
                @params
            );
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed();
        }
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        try
        {
            await Ydb.Execute(
                "DECLARE $id AS String;\n" +
                $"DELETE FROM roles_asp WHERE role_id=$id",
                new Dictionary<string, YdbValue>
                {
                    ["$id"] = YdbValue.MakeString(role.RoleId.ToByteArray())
                });
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(); // todo what if deleted but err on confirmation?
        }
        return IdentityResult.Success;
    }

    public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        var userIdGuid = Guid.Parse(roleId);
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT * FROM roles_asp WHERE role_id = $id;",
            new Dictionary<string, YdbValue>()
            {
                ["$id"] = YdbValue.MakeString(userIdGuid.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? Role.FromYdbRow(resultSet.Rows[0]) : null;

    }

    public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        var response = await Ydb.Execute(
            $"DECLARE $normalized_name AS Utf8;\n" +
            $"SELECT * FROM roles_asp VIEW roles_asp_by_normalized_name\n" +
            $"WHERE normalized_name = $normalized_name;",
            new Dictionary<string, YdbValue>()
            {
                ["$normalized_name"] = YdbValue.MakeUtf8(normalizedRoleName)
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? Role.FromYdbRow(resultSet.Rows[0]) : null;

    }

    public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        return role.RoleId.ToString();
    }

    public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        return role.Name;
    }

    public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        role.Name = roleName;
    }

    public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        return role.NormalizedName;
    }

    public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role));

        role.NormalizedName = normalizedName;
    }

    public void Dispose()
    {
    }
}