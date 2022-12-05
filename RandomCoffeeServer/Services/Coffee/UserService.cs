using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;

namespace RandomCoffeeServer.Services.Coffee;

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