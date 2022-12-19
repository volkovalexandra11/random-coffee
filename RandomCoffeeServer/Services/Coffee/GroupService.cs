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
        await AddUserToGroup(group.AdminUserId, group.GroupId);
    }
    
    public async Task<GroupDto?> GetGroup(Guid groupId)
    {
        return await groupRepository.FindGroup(groupId);
    }

    public async Task<Guid[]?> GetUsersInGroup(Guid groupId)
    {
        return await groupUserRepository.FindUsersInGroup(groupId);
    }
    
    public async Task<Guid[]> GetGroupsByUser(Guid userId)
    {
        return await groupUserRepository.FindGroupsByUser(userId);
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
    private readonly GroupUserRepository groupUserRepository;
}