using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Jobs;

public class PopulateWithMockDataJob
{
    public PopulateWithMockDataJob(UserService userService, GroupService groupService)
    {
        this.userService = userService;
        this.groupService = groupService;
    }
    
    public async Task Fill(CancellationToken cancellationToken)
    {
        // todo!
        return;
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