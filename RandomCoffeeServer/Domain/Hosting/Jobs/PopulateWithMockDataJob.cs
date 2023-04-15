using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Domain.Services.Coffee;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages;
using RandomCoffeeServer.Storage.Repositories.AspIdentityStorages.IdentityModel;

namespace RandomCoffeeServer.Domain.Hosting.Jobs;

public class PopulateWithMockDataJob
{
    public PopulateWithMockDataJob(
        IdentityUserStore userStore,
        GroupService groupService,
        ILogger<PopulateWithMockDataJob> log)
    {
        this.userStore = userStore;
        this.groupService = groupService;
        this.log = log;
    }

    public async Task Fill(CancellationToken cancellationToken)
    {
        log.LogInformation("Starting filling db with mock data");

        var sashaId = Guid.Parse("6b8d5161-3bce-4e03-9d83-68344a2d8567");
        var serezhaId = Guid.Parse("2bf09eff-d886-4ffc-8c66-11f3b818c2ee");
        var aidarId = Guid.Parse("f8b97237-2b08-4062-a916-77b7285e93c4");
        var vanyaId = Guid.Parse("98998fff-1b58-4364-9aa3-b0e23babb074");
        var vasyaPupkinId = Guid.Parse("43ef1000-0000-0000-0000-000000000000");
        var mishaId = Guid.Parse("7c0db301-69bf-4295-a2e0-67905c9ff390");
        var addUsers = new Task[]
        {
            AddUser(new User
                {
                    UserId = sashaId,
                    Email = "sazhev.alexandr@gmail.com",
                    FirstName = "Alexandra",
                    LastName = "Volkova",
                    ProfilePictureUrl =
                        "https://lh3.googleusercontent.com/a/AEdFTp4Fi52meAVsx_wsR3l0PEsjOt9MLOTPRA3hjDPgNQ=s96-c"
                },
                "104510970542003243147",
                cancellationToken),
            AddUser(new User
                {
                    UserId = serezhaId,
                    Email = "lugowyh.serezha@gmail.com",
                    FirstName = "Сергей",
                    LastName = "Луговых",
                    ProfilePictureUrl =
                        "https://lh3.googleusercontent.com/a/AEdFTp7ZtSUY-P40HcnSM6t3LomzFwL2VN6mRDG_mKMC=s96-c"
                },
                "113028660294942278861",
                cancellationToken),

            AddUser(new User
                {
                    UserId = Guid.Parse("f8b97237-2b08-4062-a916-77b7285e93c4"),
                    Email = "s.aidar7894@gmail.com",
                    FirstName = "Айдар",
                    LastName = "Шайхуллин",
                    ProfilePictureUrl =
                        "https://lh3.googleusercontent.com/a/AEdFTp5_ugZpUdNHfVxXwwFAQ4m7xRlQTuutXRP7TkU=s96-c"
                },
                "102332852533522884466",
                cancellationToken),
            AddUser(new User
                {
                    UserId = vanyaId,
                    Email = "ivan.s2002.mail@gmail.com",
                    FirstName = "Иван",
                    LastName = "Самсонов",
                    ProfilePictureUrl =
                        "https://lh3.googleusercontent.com/a/AEdFTp5MyMVvEThjNJQTChvWopavzuESIiHLzv3ep307bQ=s96-c",
                },
                "101315306617911739546",
                cancellationToken),
            AddUser(new User
                {
                    UserId = vasyaPupkinId,
                    Email = "vasya.pupkin@mail.fake",
                    FirstName = "Вася",
                    LastName = "Пупкин",
                    ProfilePictureUrl =
                        "https://avatars.dzeninfra.ru/get-zen_doc/1337093/pub_5eceb0ed6079e31d4ed971c4_5eceb18c92e0f61ff3249dc6/scale_1200"
                },
                "im-fake-google-key",
                cancellationToken),
            AddUser(new User
                {
                    UserId = mishaId,
                    Email = "muxa09032002@gmail.com",
                    FirstName = "Михаил",
                    LastName = "Романенко",
                    ProfilePictureUrl =
                        "https://lh3.googleusercontent.com/a/AGNmyxatQ0FMHdTnY1cHQDdUftzA9odWVbQv1C4r3Aq6=s96-c"
                },
                "im-fake-google-key",
                cancellationToken),
        };

        var group1Id = Guid.Parse("9f048110-0000-0000-0000-000000000000");
        var group2Id = Guid.Parse("9f048120-0000-0000-0000-000000000000");
        var group3Id = Guid.Parse("9f048130-0000-0000-0000-000000000000");
        var group4Id = Guid.Parse("9f048140-0000-0000-0000-000000000000");

        var groups = new GroupService.CreateGroupDto[]
        {
            new()
            {
                GroupId = group1Id,
                Name = "Test group",
                AdminUserId = sashaId,
                GroupPictureUrl = "/img/picture1.jpg"
            },
            new()
            {
                GroupId = group2Id,
                Name = "Moon group",
                AdminUserId = serezhaId,
                GroupPictureUrl = "/img/picture2.jpg"
            },
            new()
            {
                GroupId = group3Id,
                Name = "Группа Вани",
                AdminUserId = vanyaId,
                GroupPictureUrl = "/img/picture.jpg"
            },
            new()
            {
                GroupId = group4Id,
                Name = "Test group2",
                AdminUserId = vasyaPupkinId,
                GroupPictureUrl = "/img/avatar.jpg"
            }
        };
        var addGroups = groups.Select(group => groupService.AddGroup(group)).ToArray();

        var addUsersToGroups = new Task[]
        {
            groupService.AddParticipantToGroup(sashaId, group1Id),
            groupService.AddParticipantToGroup(sashaId, group2Id),
            groupService.AddParticipantToGroup(sashaId, group3Id),

            groupService.AddParticipantToGroup(serezhaId, group1Id),
            groupService.AddParticipantToGroup(serezhaId, group2Id),
            groupService.AddParticipantToGroup(serezhaId, group3Id),

            groupService.AddParticipantToGroup(aidarId, group1Id),
            groupService.AddParticipantToGroup(aidarId, group2Id),
            groupService.AddParticipantToGroup(aidarId, group3Id),

            groupService.AddParticipantToGroup(vanyaId, group1Id),
            groupService.AddParticipantToGroup(vanyaId, group2Id),
            groupService.AddParticipantToGroup(vanyaId, group3Id),

            groupService.AddParticipantToGroup(vasyaPupkinId, group1Id),
            
            groupService.AddParticipantToGroup(mishaId, group1Id),
            groupService.AddParticipantToGroup(mishaId, group2Id),
            groupService.AddParticipantToGroup(mishaId, group3Id),
        };

        await Task.WhenAll(addUsers.Concat(addGroups).Concat(addUsersToGroups));

        log.LogInformation("Successfully filled db with with mock data");
    }

    private async Task AddUser(User user, string googleKey, CancellationToken cancellationToken)
    {
        var identityUser = new IdentityCoffeeUser()
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePictureUrl = user.ProfilePictureUrl,

            UserName = user.Email,
            NormalizedUserName = user.Email.ToUpperInvariant()
        };
#pragma warning disable CS0618
        await userStore.ReplaceAsync(identityUser, cancellationToken);
        await userStore.ReplaceLoginAsync(
            identityUser, new UserLoginInfo("Google", googleKey, "Google"), cancellationToken);
#pragma warning restore CS0618
    }

    private readonly GroupService groupService;
    private readonly ILogger<PopulateWithMockDataJob> log;
    private readonly IdentityUserStore userStore;
}