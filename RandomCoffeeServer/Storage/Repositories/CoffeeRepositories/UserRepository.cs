using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk.Table;
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
            .Replace(user.ToYdb()) // todo insert (now mock job would fail)
            .ExecuteNonData(Ydb);
    }
    
    public async Task UpdateUser(User user)
    {
        await Users
            .Replace(user.ToYdb())  // todo if exists
            .ExecuteNonData(Ydb);
    }

    public async Task<User?> FindUser(Guid userId)
    {
        var users = await Users
            .Select()
            .Where("user_id", YdbValue.MakeString(userId.ToByteArray()))
            .ExecuteData(Ydb);

        return users.SingleOrDefault(User.FromYdbRow);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        var users = await Users
            .Select()
            .ViewByColumn("email")
            .Where("email", YdbValue.MakeUtf8(email))
            .ExecuteData(Ydb);

        return users.SingleOrDefault(User.FromYdbRow);
    }

    public async Task DeleteUser(Guid userId)
    {
        await Users
            .Delete()
            .Where("user_id", YdbValue.MakeString(userId.ToByteArray()))
            .ExecuteNonData(Ydb);
    }
}