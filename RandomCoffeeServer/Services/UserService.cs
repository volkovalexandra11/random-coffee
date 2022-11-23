using RandomCoffee.Dtos;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee;

public class UserService
{
    private readonly YdbService ydbService;

    public UserService(YdbService ydbService)
    {
        this.ydbService = ydbService;
    }

    public async Task AddUser(UserDto user)
    {
        var users = Schema.Tables.Single(t => t.TableName == "users");
        var declare = QueryBuilder.QueryBuilder.AsTableDeclare(users);
        await ydbService.Execute($"{declare};\nUPSERT INTO users SELECT * FROM AS_TABLE($data);", new Dictionary<string, YdbValue>()
        {
            { "data", user.ToYdb() }
        });
    } 
}