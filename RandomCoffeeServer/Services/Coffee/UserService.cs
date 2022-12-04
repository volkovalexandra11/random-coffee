using RandomCoffee.Dtos;
using RandomCoffee.Repositories;
using RandomCoffee.schema;
using Ydb.Sdk.Value;

namespace RandomCoffee;

public class UserService
{
    public UserService(UserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task AddUser(UserDto user)
    {
        await userRepository.AddUser(user);
    }

    private readonly UserRepository userRepository;
}