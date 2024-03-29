﻿using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

public class UserRepository : RepositoryBase
{
    private YdbTable Users { get; }

    public UserRepository(YdbService ydb)
        : base(ydb)
    {
        Users = Schema.Users;
    }

    public async Task AddUser(User user)
    {
        await Users
            .Insert(user.ToYdb())
            .ExecuteNonData(Ydb);
    }

    [Obsolete("for mock data only")]
    public async Task ReplaceUser(User user)
    {
        await Users
            .Replace(user.ToYdb())
            .ExecuteNonData(Ydb);
    }

    public async Task UpdateUser(User user)
    {
        await Users
            .Replace(user.ToYdb()) // todo if exists
            .ExecuteNonData(Ydb);
    }

    public async Task UpdateUser(Guid userId, User user)
    {
        var whereParameters = new Dictionary<string, YdbValue>
        {
            {
                "user_id",
                userId.ToYdb()
            }
        };
        var setParameters = user.ToYdb();
        setParameters.Remove("user_id");
        await Users.Update(setParameters, whereParameters).ExecuteData(Ydb);
    }

    public async Task<User?> FindUser(Guid userId)
    {
        var users = await Users
            .Select()
            .Where("user_id", userId.ToYdb())
            .ExecuteData(Ydb);

        return users.SingleOrNull(User.FromYdbRow);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        var users = await Users
            .Select()
            .ViewByColumn("email")
            .Where("email", email.ToYdb())
            .ExecuteData(Ydb);

        return users.SingleOrNull(User.FromYdbRow);
    }

    public async Task DeleteUser(Guid userId)
    {
        await Users
            .Delete()
            .Where("user_id", userId.ToYdb())
            .ExecuteNonData(Ydb);
    }
}