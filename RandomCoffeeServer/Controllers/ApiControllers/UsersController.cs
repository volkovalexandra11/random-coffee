using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Domain.Services.Coffee;

namespace RandomCoffeeServer.Controllers.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    public UsersController(UserService userService, GroupService groupService)
    {
        this.userService = userService;
        this.groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var users = await groupService.GetParticipantsInGroup(groupId);
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