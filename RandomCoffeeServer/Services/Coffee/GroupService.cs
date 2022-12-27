using RandomCoffeeServer.Controllers.GroupsControllerDtos;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Services.Coffee;

public class GroupService
{
    public GroupService(
        GroupRepository groupRepository,
        GroupUserRepository groupUserRepository,
        UserRepository userRepository)
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

    public async Task<GroupWithParticipantsDto?> GetGroupWithParticipants(Guid groupId)
    {
        var group = await GetGroup(groupId);
        if (group is null)
            return null;

        var participantsIds = await GetUsersInGroup(groupId);

        if (participantsIds is null)
            return null;

        var participants = await Task.WhenAll(participantsIds.Select(userId => userRepository.FindUser(userId)));
        var participantsAsDto = participants
            .Where(user => user is not null)
            .Select(user => new ParticipantDto(user!));

        return new GroupWithParticipantsDto(group, participantsAsDto);
    }

    public async Task<Guid[]?> GetUsersInGroup(Guid groupId)
    {
        return await groupUserRepository.FindUsersInGroup(groupId);
    }
    
    public async Task<int?> GetParticipantsCountInGroup(Guid groupId)
    {
        return await groupUserRepository.GetParticipantsCount(groupId);
    }

    public async Task<IEnumerable<ShortFormatGroupDto>> GetGroupsByUser(Guid userId)
    {
        var groupIds = await groupUserRepository.FindGroupsByParticipant(userId);
        var groupsDto =
            await Task.WhenAll(groupIds.Select(async groupId => await GetGroup(groupId)));
        return await Task.WhenAll(groupsDto
            .Where(group => group is not null)
            .Select(async group => new ShortFormatGroupDto(group!.GroupId, group.Name, await GetParticipantsCountInGroup(group.GroupId))));
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