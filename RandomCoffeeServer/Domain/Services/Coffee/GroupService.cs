using System.Net;
using System.Net.Mail;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Storage.Repositories.CoffeeRepositories;

namespace RandomCoffeeServer.Domain.Services.Coffee;

public class GroupService
{
    public GroupService(
        GroupRepository groupRepository,
        GroupUserRepository groupUserRepository,
        UserRepository userRepository, 
        EmailService emailService)
    {
        this.groupRepository = groupRepository;
        this.groupUserRepository = groupUserRepository;
        this.userRepository = userRepository;
        this.emailService = emailService;
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

    public async Task<GroupWithParticipantsDto?> GetGroupWithParticipants(Guid groupId)
    {
        var groupWithUsers = await GetGroupWithUsersModels(groupId);
        var participantsAsDto = groupWithUsers.Users
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
            GroupId = groupWithUsers.GroupId,
            Admin = participantsAsDto.First(participant => participant.UserId == groupWithUsers.Admin.UserId),
            Name = groupWithUsers.Name,
            IsPrivate = groupWithUsers.IsPrivate,
            Participants = participantsAsDto,
            NextRoundDate = DateTime.Now
        };
    }
    
    public async Task<GroupWithUsersModels?> GetGroupWithUsersModels(Guid groupId)
    {
        var group = await GetGroup(groupId);
        if (group is null)
            return null;

        var participantsIds = await groupUserRepository.FindUsersInGroup(groupId);

        if (participantsIds is null)
            return null;

        var participants = await Task.WhenAll(participantsIds.Select(userId => userRepository.FindUser(userId)));
        var users = participants
            .Where(user => user is not null)
            .ToList();

        return new GroupWithUsersModels()
        {
            GroupId = group.GroupId,
            Admin = users.First(user => user.UserId == group.AdminUserId),
            Name = group.Name,
            IsPrivate = group.IsPrivate,
            Users = users,
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
        await groupUserRepository.AddToGroup(userId: userId, groupId: groupId);
    }
    
    public async Task StartRound(Guid groupId)
    {
        var groupWithUsers = await GetGroupWithUsersModels(groupId);
        var users = new HashSet<User>(groupWithUsers.Users);
        var usersInPairs = new HashSet<Tuple<User, User>>();
        var r = new Random();
        if (users.Count % 2 == 1)
        {
            var randomNumber = r.Next(users.Count);
            var randomUser = users.ElementAt(randomNumber);
            users.Remove(randomUser);
            emailService.SendMessage(groupWithUsers.Name, randomUser, null);
        }
        while (users.Count > 1)
        {
            var firstRandomNumber = r.Next(users.Count);
            int secondRandomNumber;
            do secondRandomNumber = r.Next(users.Count); while (secondRandomNumber == firstRandomNumber);
            var firstUser = users.ElementAt(firstRandomNumber);
            var secondUser = users.ElementAt(secondRandomNumber);
            usersInPairs.Add(new Tuple<User, User>(firstUser, secondUser));
            users.Remove(firstUser);
            users.Remove(secondUser);
        }

        foreach (var usersPair in usersInPairs)
        {
            var firstUser = usersPair.Item1;
            var secondUser = usersPair.Item2;
            emailService.SendMessage(groupWithUsers.Name, firstUser, secondUser);
            emailService.SendMessage(groupWithUsers.Name, secondUser, firstUser);
        }
    }


    private readonly GroupRepository groupRepository;
    private readonly UserRepository userRepository;
    private readonly GroupUserRepository groupUserRepository;
    private readonly EmailService emailService;
}