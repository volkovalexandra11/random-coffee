using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Storage.Repositories;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

namespace RandomCoffeeServer.Domain.Services.Coffee;

public class UserService
{
    public UserService(UserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task AddUser(User user)
    {
        await userRepository.AddUser(user);
    }

    public async Task<User?> GetUser(Guid userId)
    {
        return await userRepository.FindUser(userId);
    }

    private readonly UserRepository userRepository;
}