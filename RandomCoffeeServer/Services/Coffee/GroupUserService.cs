using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
using Ydb.Sdk.Client;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Services.Coffee;

public class GroupUserService
{
    public GroupUserService(
        GroupUserRepository groupUserRepository)
    {
        this.groupUserRepository = groupUserRepository;
    }

    public async Task AddUserToGroup(GroupUserDto groupUser)
    {
        await groupUserRepository.AddToGroup(groupUser);
    }
    
    private readonly GroupUserRepository groupUserRepository;
}