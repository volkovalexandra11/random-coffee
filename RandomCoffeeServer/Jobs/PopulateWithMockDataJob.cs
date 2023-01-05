using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Jobs;

public class PopulateWithMockDataJob
{
    public PopulateWithMockDataJob(
        UserService userService, 
        GroupService groupService, 
        RoundService roundService,
        ScheduleService scheduleService)
    {
        this.userService = userService;
        this.groupService = groupService;
        this.roundService = roundService;
        this.scheduleService = scheduleService;
    }
    
    public async Task Fill(CancellationToken cancellationToken)
    {
        var user1Id = Guid.Parse("43ef1000-0000-0000-0000-000000000000");
        var user2Id = Guid.Parse("43ef2000-0000-0000-0000-000000000000");
        var user3Id = Guid.Parse("43ef3000-0000-0000-0000-000000000000");

        var group1Id = Guid.Parse("9f048110-0000-0000-0000-000000000000");
        var group2Id = Guid.Parse("9f048120-0000-0000-0000-000000000000");
        var group3Id = Guid.Parse("9f048130-0000-0000-0000-000000000000");
        var group4Id = Guid.Parse("9f048140-0000-0000-0000-000000000000");
        
        var round1Id = Guid.Parse("58fe1000-0000-0000-0000-000000000000");
        var round2Id = Guid.Parse("58fe2000-0000-0000-0000-000000000000");
        var round3Id = Guid.Parse("58fe3000-0000-0000-0000-000000000000");
        var round4Id = Guid.Parse("58fe4000-0000-0000-0000-000000000000");
        
        var users = new UserDto[]
        {
            new UserDto()
            {
                UserId = user1Id,
                Email = "vasya.pupkin@mail.ru",
                FirstName = "Вася",
                LastName = "Пупкин",
                ProfilePictureUrl = null
            },
            new UserDto()
            {
                UserId = user2Id,
                Email = "pasya.vutkin@mail.ru",
                FirstName = "Пася",
                LastName = "Вуткин",
                ProfilePictureUrl = "/static/img/avatar.jpg"
            },
            new UserDto()
            {
                UserId = user3Id,
                Email = "user3@yandex.ru",
                FirstName = "Some",
                LastName = "One",
                ProfilePictureUrl = null
            }
        };

        var groups = new GroupDto[]
        {
            new GroupDto()
            {
                GroupId = group1Id,
                Name = "Test group",
                AdminUserId = user2Id,
            },
            new GroupDto()
            {
                GroupId = group2Id,
                Name = "Moon group",
                AdminUserId = user1Id
            },
            new GroupDto()
            {
                GroupId = group3Id,
                Name = "Sunshine group",
                AdminUserId = user1Id
            },
            new GroupDto()
            {
                GroupId = group4Id,
                Name = "Test group2",
                AdminUserId = user1Id
            }
        };
        
        var rounds = new RoundDto[]
        {
            new RoundDto()
            {
                RoundId = round1Id,
                GroupId = group1Id,
                Date = DateTime.Now,
                WasNotifiedAbout = true
            },
            new RoundDto()
            {
                RoundId = round2Id,
                GroupId = group2Id,
                Date = DateTime.Now,
                WasNotifiedAbout = true
            },
            new RoundDto()
            {
                RoundId = round3Id,
                GroupId = group3Id,
                Date = DateTime.Now,
                WasNotifiedAbout = true
            },
            new RoundDto()
            {
                RoundId = round4Id,
                GroupId = group4Id,
                Date = DateTime.Now,
                WasNotifiedAbout = true
            }
        };

        var schedules = new ScheduleDto[]
        {
            new ScheduleDto()
            {
                GroupId = group1Id,
                IntervalDays = 1,
                NextRoundId = round1Id
            },
            new ScheduleDto()
            {
                GroupId = group2Id,
                IntervalDays = 2,
                NextRoundId = round2Id
            },
            new ScheduleDto()
            {
                GroupId = group3Id,
                IntervalDays = 3,
                NextRoundId = round3Id
            },
            new ScheduleDto()
            {
                GroupId = group4Id,
                IntervalDays = 4,
                NextRoundId = round4Id
            },
        };

        await Task.WhenAll(users.Select(user => userService.AddUser(user)));
        await Task.WhenAll(groups.Select(group => groupService.AddGroup(group)));
        await Task.WhenAll(rounds.Select(round => roundService.AddRound(round)));
        await Task.WhenAll(schedules.Select(schedule => scheduleService.AddSchedule(schedule)));

        await Task.WhenAll(new Task[]
        {
            groupService.AddUserToGroup(user2Id, group1Id)
        });
    }

    private readonly UserService userService;
    private readonly GroupService groupService;
    private readonly RoundService roundService;
    private readonly ScheduleService scheduleService;
}