using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Helpers;
using RandomCoffeeServer.Services.YandexCloud.Ydb;
using Ydb.Sdk.Client;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Repositories.IdentityStorageProvider;

public class CoffeeUserStore : RepositoryBase, IUserStore<User>
{
    public CoffeeUserStore(YdbService ydb)
        : base(ydb, "")
    {
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        // todo null-check        

        var @params = YdbConverter.ToDataParams(user.ToYdb());
        try
        {
            await Ydb.Execute(
                $"{DeclareStatement}\n" +
                "INSERT INTO users_asp SELECT * FROM AS_TABLE($data);",
                @params
            );
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed();
        }
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var @params = YdbConverter.ToDataParams(user.ToYdb());
        try
        {
            await Ydb.Execute(
                $"{DeclareStatement}\n" +
                "REPLACE INTO users_asp SELECT * FROM AS_TABLE($data);", // todo UPDATE?
                @params
            );
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed();
        }
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        try
        {
            await Ydb.Execute(
                "DECLARE $id AS String;\n" +
                $"DELETE FROM users_asp WHERE user_id=$id",
                new Dictionary<string, YdbValue>
                {
                    ["$id"] = YdbValue.MakeString(user.UserId.ToByteArray())
                });
        }
        catch (Exception ex)
        {
            return IdentityResult.Failed(); // todo what if deleted but err on confirmation?
        }
        return IdentityResult.Success;
    }

    public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var userIdGuid = Guid.Parse(userId);
        var response = await Ydb.Execute(
            $"DECLARE $id AS String;\n" +
            $"SELECT * FROM users_asp WHERE user_id = $id;",
            new Dictionary<string, YdbValue>()
            {
                ["$id"] = YdbValue.MakeString(userIdGuid.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? User.FromYdbRow(resultSet.Rows[0]) : null;
    }

    public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var response = await Ydb.Execute(
            $"DECLARE $normalized_username AS Utf8;\n" +
            $"SELECT * FROM users_asp VIEW users_asp_by_normalized_username\n" +
            $"WHERE normalized_username = $normalized_username;",
            new Dictionary<string, YdbValue>()
            {
                ["$normalized_username"] = YdbValue.MakeUtf8(normalizedUserName)
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        return resultSet.Rows.Count == 1 ? User.FromYdbRow(resultSet.Rows[0]) : null;
    }

    public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.UserId.ToString();
    }

    public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.UserId.ToString();
    }

    public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.UserName = userName;
    }

    public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        return user.NormalizedUserName;
    }

    public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        user.NormalizedUserName = normalizedName;
    }

    public void Dispose()
    {
    }
}