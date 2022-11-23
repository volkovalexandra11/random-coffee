using Ydb.Sdk.Value;

namespace RandomCoffee.Dtos;

public class UserDto
{
    public ulong UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    
    public YdbValue ToYdb()
    {
        return YdbValue.MakeStruct(new Dictionary<string, YdbValue>
        {
            ["user_id"] = YdbValue.MakeUint64(UserId),
            ["email"] = YdbValue.MakeUtf8(Email),
            ["first_name"] = YdbValue.MakeUtf8(FirstName),
            ["last_name"] = YdbValue.MakeUtf8(LastName)
        });
    }
}