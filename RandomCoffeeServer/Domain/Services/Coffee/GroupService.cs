using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.Repositories;
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
        await AddUserToGroup(group.AdminUserId, group.GroupId);
    }

    public async Task<Group?> GetGroup(Guid groupId)
    {
        return await groupRepository.FindGroup(groupId);
    }

    public async Task<GroupWithParticipantsDto?> GetGroupWithParticipants(Guid groupId)
    {
        var group = await GetGroup(groupId);
        if (group is null)
            return null;

        var participantsIds = await groupUserRepository.FindUsersInGroup(groupId);

        if (participantsIds is null)
            return null;

        var participants = await Task.WhenAll(participantsIds.Select(userId => userRepository.FindUser(userId)));
        var participantsAsDto = participants
            .Where(user => user is not null)
            .Select(user => new ParticipantDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = user.ProfilePictureUrl
            })
            .ToList();

        return new GroupWithParticipantsDto
        {
            GroupId = group.GroupId,
            Admin = participantsAsDto.First(participant => participant.UserId == group.AdminUserId),
            Name = group.Name,
            IsPrivate = group.IsPrivate,
            Participants = participantsAsDto,
            NextRoundDate = DateTime.Now
        };
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