using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Domain.Services.Coffee;

namespace RandomCoffeeServer.Domain.Hosting.Jobs;

public class PopulateWithMockDataJob
{
    public PopulateWithMockDataJob(UserService userService, GroupService groupService)
    {
        this.userService = userService;
        this.groupService = groupService;
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

        var groups = new Group[]
        {
            new Group()
            {
                GroupId = group1Id,
                Name = "Test group",
                AdminUserId = user2Id,
            },
            new Group()
            {
                GroupId = group2Id,
                Name = "Moon group",
                AdminUserId = user1Id
            },
            new Group()
            {
                GroupId = group3Id,
                Name = "Sunshine group",
                AdminUserId = user1Id
            },
            new Group()
            {
                GroupId = group4Id,
                Name = "Test group2",
                AdminUserId = user1Id
            }
        };

        await Task.WhenAll(users.Select(user => userService.AddUser(user)));
        await Task.WhenAll(groups.Select(group => groupService.AddGroup(group)));

        await Task.WhenAll(new Task[]
        {
            groupService.AddUserToGroup(user2Id, group1Id)
        });
    }

    private readonly UserService userService;
    private readonly GroupService groupService;
}