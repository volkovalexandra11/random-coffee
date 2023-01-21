using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

namespace RandomCoffeeServer.Domain.Services.Coffee;

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

    public async Task AddGroup(Group group)
    {
        await groupRepository.AddGroup(group);
        await groupUserRepository.AddToGroup(userId: group.AdminUserId, groupId: group.GroupId);
    }

    public async Task<Group?> GetGroup(Guid groupId)
    {
        return await groupRepository.FindGroup(groupId);
    }

    public async Task<(Group? group, User[]? participants)> GetGroupWithParticipantModels(Guid groupId)
    {
        var group = await GetGroup(groupId);
        if (group is null)
            return (null, null);

        var participantsIds = await groupUserRepository.FindUsersInGroup(groupId);

        if (participantsIds is null)
            throw new InvalidProgramException($"No users found in group {group.Name} ({groupId})");

        var participants = await Task.WhenAll(participantsIds.Select(userId => userRepository.FindUser(userId)));
        if (participants.Any(user => user is null))
            throw new InvalidProgramException($"Couldn't find some users in group");

        return (group, participants)!;
    }

    public async Task<Guid[]?> GetParticipantsInGroup(Guid groupId)
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
            .Select(async group => new ShortFormatGroupDto
            {
                GroupId = group!.GroupId,
                Name = group.Name,
                ParticipantsCount = await GetParticipantsCountInGroup(group.GroupId) ?? 0,
                NextRoundDate = DateTime.Now
            }));
    }

    public async Task AddParticipantToGroup(Guid userId, Guid groupId)
    {
        await groupUserRepository.AddToGroup(userId: userId, groupId: groupId);
    }
    
    public async Task<DeleteParticipantResult> TryDeleteParticipantFromGroup(Guid userId, Guid groupId)
    {
        var group = await groupRepository.FindGroup(groupId);
        if (group is null)
            return DeleteParticipantResult.NoGroupError;
        if (group.AdminUserId == userId)
            return DeleteParticipantResult.ParticipantIsAdminError;
        
        await groupUserRepository.DeleteFromGroup(userId: userId, groupId: groupId);
        return DeleteParticipantResult.Success;
    }

    private readonly GroupRepository groupRepository;
    private readonly UserRepository userRepository;
    private readonly GroupUserRepository groupUserRepository;

    public enum DeleteParticipantResult
    {
        Success,
        NoGroupError,
        ParticipantIsAdminError
    }
}