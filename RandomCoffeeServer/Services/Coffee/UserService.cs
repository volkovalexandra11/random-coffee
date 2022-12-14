using System.Net.Sockets;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Services.Coffee;

public class UserService
{
    public UserService(UserRepository userRepository, GroupUserRepository groupUserRepository)
    {
        this.userRepository = userRepository;
        this.groupUserRepository = groupUserRepository;
    }

    public async Task AddUser(UserDto user)
    {
        await userRepository.AddUser(user);
    }
    
    public async Task<List<string>> GetGroups(Guid id)
    {
        var a = await groupUserRepository.GetGroups(id);
        var groupsId = new List<string>();
        foreach (var row in a.Rows)
        {
            groupsId.Add(new Guid(row["group_id"].GetOptionalString()).ToString());
        }

        return groupsId;
    }
    
    public async Task<UserDto> GetUser(Guid id)
    {
        var user =  await userRepository.GetUser(id);
        var userDto = new UserDto
        {
            UserId = new Guid(user.Rows[0]["user_id"].GetOptionalString()),
            FirstName = (string)user.Rows[0]["first_name"],
            LastName = (string)user.Rows[0]["last_name"],
            Email = (string)user.Rows[0]["email"]
        };
        return userDto;
    }

    private readonly UserRepository userRepository;
    private readonly GroupUserRepository groupUserRepository;
}