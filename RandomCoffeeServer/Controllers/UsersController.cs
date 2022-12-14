using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.UsersControllerDtos;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    public UsersController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
    {
        var userId = Guid.NewGuid();

        await userService.AddUser(
            new UserDto
            {
                UserId = userId,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email
            });

        return Ok();
    }
    
    [HttpGet("{id}/groups")]
    public async Task<ActionResult> GetUserGroups(Guid id)
    {
        return Ok(new Dictionary<string, List<string>>{["groups"] = await userService.GetGroups(id)});
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUser(Guid id)
    {
        return Ok(await userService.GetUser(id));
    }

    private readonly UserService userService;
}