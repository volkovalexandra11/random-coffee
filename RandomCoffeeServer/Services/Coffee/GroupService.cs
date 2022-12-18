using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;

namespace RandomCoffeeServer.Services.Coffee;

public class GroupService
{
    public GroupService(
        GroupRepository groupRepository,
        GroupUserRepository groupUserRepository)
    {
        this.groupRepository = groupRepository;
        this.groupUserRepository = groupUserRepository;
    }

    public async Task AddGroup(GroupDto group)
    {
        await groupRepository.AddGroup(group);
    }
    
    public async Task<GroupDto> GetGroup(Guid id)
    {
        var group = await groupRepository.GetGroup(id);
        var users = await GetUsers(id);
        return GroupDto.FromYdbRow(group.Rows[0], users);
    }

    // public async Task<List<GroupDto>> GetAllGroups()
    // {
        // var groups = await groupRepository.GetAllGroups();
        // var groupsUsers = groups.Select(group => await GetUsers(group))
    // }

    public async Task<List<string>> GetUsers(Guid id)
    {
        var c = await groupUserRepository.GetUsers(id);
        var groupsId = new List<string>();
        foreach (var row in c.Rows)
        {
            groupsId.Add(new Guid(row["user_id"].GetOptionalString()).ToString());
        }

        return groupsId;
    }
    
    private readonly GroupRepository groupRepository;
    private readonly GroupUserRepository groupUserRepository;
}