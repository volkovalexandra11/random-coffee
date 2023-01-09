using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;

public class CoffeeUserStore : IUserLoginStore<User>
{
    private readonly YdbService ydb;
    private readonly string userDeclareStatement;
    private readonly string userLoginsDeclareStatement;

    public CoffeeUserStore(YdbService ydb)
    {
        this.ydb = ydb;
        this.userDeclareStatement = QueryBuilder.AsTableDeclare(Schema.UsersAsp);
        this.userLoginsDeclareStatement = QueryBuilder.AsTableDeclare(Schema.UserLoginsAsp);
    }

    public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        // todo null-check        

        var userLoginDto = new UserLoginDto()
        {
            LoginProvider = login.LoginProvider,
            ProviderKey = login.ProviderKey,
            ProviderDisplayName = login.ProviderDisplayName,
            UserId = user.UserId
        };

        var @params = YdbConverter.ToDataParams(userLoginDto.ToYdb());
        await ydb.Execute(
            $"{userLoginsDeclareStatement}\n" +
            "INSERT INTO user_logins_asp SELECT * FROM AS_TABLE($data);",
            @params
        );
    }

    public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        await ydb.Execute(
            "DECLARE $provider AS Utf8;\n" +
            "DECLARE $provider_key AS Utf8;\n" +
            $"DELETE FROM user_logins_asp\n" +
            $"WHERE login_provider=$provider AND provider_key=$provider_key;",
            new Dictionary<string, YdbValue>
            {
                ["$provider"] = YdbValue.MakeUtf8(loginProvider),
                ["$provider_key"] = YdbValue.MakeUtf8(providerKey)
            });
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
    {
        var response = await ydb.Execute(
            $"DECLARE $user_id AS String;\n" +
            $"SELECT * FROM user_logins_asp " +
            $"VIEW user_logins_asp_by_user_id " +
            $"WHERE user_id = $user_id;",
            new Dictionary<string, YdbValue>()
            {
                ["$user_id"] = YdbValue.MakeString(user.UserId.ToByteArray())
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];

        return resultSet.Rows.Select(UserLoginDto.FromYdbRow)
            .Select(dto => new UserLoginInfo(dto.LoginProvider, dto.ProviderKey, dto.ProviderDisplayName))
            .ToList();
    }

    public async Task<User?> FindByLoginAsync(string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        var response = await ydb.Execute(
            "DECLARE $provider AS Utf8;\n" +
            "DECLARE $provider_key AS Utf8;\n" +
            $"SELECT user_id FROM user_logins_asp\n" +
            $"WHERE login_provider=$provider AND provider_key=$provider_key;",
            new Dictionary<string, YdbValue>
            {
                ["$provider"] = YdbValue.MakeUtf8(loginProvider),
                ["$provider_key"] = YdbValue.MakeUtf8(providerKey)
            });
        response.Status.EnsureSuccess();
        var queryResponse = (ExecuteDataQueryResponse)response;
        var resultSet = queryResponse.Result.ResultSets[0];
        if (resultSet.Rows.Count == 0)
            return null;

        var userId = resultSet.Rows[0]["user_id"].GetNonNullGuid().ToString();
        return await FindByIdAsync(userId, cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        // todo null-check        

        var @params = YdbConverter.ToDataParams(user.ToYdb());
        try
        {
            await ydb.Execute(
                $"{userDeclareStatement}\n" +
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
            await ydb.Execute(
                $"{userDeclareStatement}\n" +
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
            await ydb.Execute(
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

    public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var userIdGuid = Guid.Parse(userId);
        var response = await ydb.Execute(
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

    public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var response = await ydb.Execute(
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