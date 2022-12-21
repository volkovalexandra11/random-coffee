using System.Net.Sockets;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
using Ydb.Sdk.Value;

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

    public async Task<UserDto?> GetUser(Guid userId)
    {
        return await userRepository.FindUser(userId);
    }

    private readonly UserRepository userRepository;
}