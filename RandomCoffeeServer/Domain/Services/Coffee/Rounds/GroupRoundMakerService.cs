using RandomCoffeeServer.Domain.Services.Coffee.Email;
using RandomCoffeeServer.Domain.Services.Coffee.UserMatching;

namespace RandomCoffeeServer.Domain.Services.Coffee.Rounds;

// Всегда проводит раунд в группе
public class GroupRoundMakerService
{
    private readonly GroupService groupService;
    private readonly IUserMatcher userMatcher;
    private readonly IMatchNotifier matchNotifier;

    public GroupRoundMakerService(
        GroupService groupService,
        IUserMatcher userMatcher,
        IMatchNotifier matchNotifier
    )
    {
        this.groupService = groupService;
        this.userMatcher = userMatcher;
        this.matchNotifier = matchNotifier;
    }

    public async Task MakeRound(Guid groupId)
    {
        var (group, participants) = await groupService.GetGroupWithParticipantModels(groupId);
        if (group is null || participants is null)
            throw new ArgumentException($"Couldn't find group {groupId} to make round in.");

        foreach (var (userId1, userId2) in userMatcher
                     .GetUserMatches(participants.Select(user => user.UserId).ToArray()))
        {
            var user1 = participants.Single(user => user.UserId == userId1);
            var user2 = participants.SingleOrDefault(user => user.UserId == userId2);

            await matchNotifier.Notify(group, user1, user2);
            if (user2 != null)
                await matchNotifier.Notify(group, user2, user1);
        }
    }
}