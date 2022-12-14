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
        var group =  await groupRepository.GetGroup(id);
        var groupDto = new GroupDto
        {
            GroupId = new Guid(group.Rows[0]["group_id"].GetOptionalString()),
            AdminUserId = new Guid(group.Rows[0]["admin_user_id"].GetOptionalString()),
            Name = (string)group.Rows[0]["name"],
            IsPrivate = group.Rows[0]["is_private"].GetOptionalInt32() != 0,
            Users = await GetUsers(id)
        };
        return groupDto;
    }

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