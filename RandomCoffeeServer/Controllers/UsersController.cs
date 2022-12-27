using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.UsersControllerDtos;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    public UsersController(UserService userService, GroupService groupService)
    {
        this.userService = userService;
        this.groupService = groupService;
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
                Email = createUserDto.Email,
                ProfilePictureUrl = createUserDto.ProfilePictureUrl
            });

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var users = await groupService.GetUsersInGroup(groupId);
        if (users is null)
            return NotFound();

        return Ok(users);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult> GetUser(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest();

        var user = await userService.GetUser(userId);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    private readonly UserService userService;
    private readonly GroupService groupService;
}