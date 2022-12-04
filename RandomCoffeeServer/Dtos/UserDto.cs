using Ydb.Sdk.Value;

namespace RandomCoffee.Dtos;

public class UserDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    
    public Dictionary<string, YdbValue> ToYdb()
    {
        return new Dictionary<string, YdbValue>
        {
            ["user_id"] = YdbValue.MakeString(UserId.ToByteArray()),
            ["email"] = YdbValue.MakeUtf8(Email),
            ["first_name"] = YdbValue.MakeUtf8(FirstName),
            ["last_name"] = YdbValue.MakeUtf8(LastName)
        };
    }
}