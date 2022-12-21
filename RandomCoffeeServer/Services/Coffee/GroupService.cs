using RandomCoffeeServer.Controllers.GroupsControllerDtos;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;

namespace RandomCoffeeServer.Services.Coffee;

public class GroupService
{
    public GroupService(
        GroupRepository groupRepository,
        GroupUserRepository groupUserRepository, UserRepository userRepository)
    {
        this.groupRepository = groupRepository;
        this.groupUserRepository = groupUserRepository;
        this.userRepository = userRepository;
    }

    public async Task AddGroup(GroupDto group)
    {
        await groupRepository.AddGroup(group);
        await AddUserToGroup(group.AdminUserId, group.GroupId);
    }

    public async Task<GroupDto?> GetGroup(Guid groupId)
    {
        return await groupRepository.FindGroup(groupId);
    }

    public async Task<GetGroupDto?> GetGroupWithUsers(Guid groupId)
    {
        var group = await GetGroup(groupId);
        if (group is null)
        {
            return null;
        }

        var users = GetUsersInGroup(groupId).Result!.Select(userId => userRepository.FindUser(userId).Result);

        return new GetGroupDto(group, users!);
    }

    public async Task<Guid[]?> GetUsersInGroup(Guid groupId)
    {
        return await groupUserRepository.FindUsersInGroup(groupId);
    }

    public async Task<IEnumerable<UserGroupDto>> GetGroupsByUser(Guid userId)
    {
        return groupUserRepository.FindGroupsByUser(userId).Result.Select(groupId => GetGroupWithUsers(groupId).Result)
            .Where(group => group is not null).Select(group => new UserGroupDto(group!));
    }

    public async Task AddUserToGroup(Guid userId, Guid groupId)
    {
        await groupUserRepository.AddToGroup(new GroupUserDto
        {
            UserId = userId,
            GroupId = groupId
        });
    }

    private readonly GroupRepository groupRepository;
    private readonly UserRepository userRepository;
    private readonly GroupUserRepository groupUserRepository;
}