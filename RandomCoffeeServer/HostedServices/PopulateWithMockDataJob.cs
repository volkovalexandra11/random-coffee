using RandomCoffee.Dtos;

namespace RandomCoffee.Services;

public class PopulateWithMockDataJob : IHostedService
{
    public PopulateWithMockDataJob(UserService userService, GroupService groupService)
    {
        this.userService = userService;
        this.groupService = groupService;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Fill();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task Fill()
    {
        // todo!
        await userService.AddUser(new UserDto
        {
            // UserId = 1,
            Email = "sergei.lugovykh@mail.ru",
            FirstName = "Сергей",
            LastName = "Луговых"
        });
        await userService.AddUser(new UserDto
        {
            // UserId = 2,
            Email = "sasha.volk@ya.ru",
            FirstName = "Саша",
            LastName = "Волк"
        });

        await groupService.AddGroup(new GroupDto
        {
            // GroupId = 1,
            Name = "Kontur.Group",
            // AdminUserId = 1,
        });
        await groupService.AddGroup(new GroupDto
        {
            // GroupId = 2,
            Name = "Home.Group",
            // AdminUserId = 2,
            IsPrivate = true
        });

        // await groupService.AddToGroup(new GroupUserDto { UserId = 1, GroupId = 1 });
        // await groupService.AddToGroup(new GroupUserDto { UserId = 2, GroupId = 2 });
        // await groupService.AddToGroup(new GroupUserDto { UserId = 2, GroupId = 2 });
    }

    private readonly UserService userService;
    private readonly GroupService groupService;
}