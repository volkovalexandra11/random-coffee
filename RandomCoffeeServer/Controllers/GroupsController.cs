using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.GroupsControllerDtos;
using RandomCoffeeServer.Dtos;
using RandomCoffeeServer.Repositories;
using RandomCoffeeServer.Services.Coffee;

namespace RandomCoffeeServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    public GroupsController(GroupService groupRepository1)
    {
        this.groupService = groupRepository1;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupDto createGroupDto)
    {
        // var userId = authTokenToUserService.Find(Http.Cookies.MyCoolAuthToken); or something like that....
        var userId = Guid.Empty; //tmp

        var groupId = Guid.NewGuid();

        await groupService.AddGroup(
            new GroupDto
            {
                Name = createGroupDto.Name,
                GroupId = groupId,
                IsPrivate = createGroupDto.IsPrivate,
                AdminUserId = userId
            });

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest();

        var groupIds = groupService.GetGroupsByUser(userId);
        return Ok(groupIds);
    }

    [HttpGet("{groupId:guid}")]
    public async Task<ActionResult> Get(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var group = await groupService.GetGroup(groupId);
        if (group is null)
            return NotFound();

        return Ok(group);
    }

    [HttpPost("{groupId:guid}/join")]
    public async Task<IActionResult> Join(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var userId = Guid.Empty; // todo

        await groupService.AddUserToGroup(userId, groupId);
        return Ok();
    }

    private readonly GroupService groupService;
}