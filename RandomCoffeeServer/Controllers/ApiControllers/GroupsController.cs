using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.ApiControllers.GroupsControllerDtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Domain.Services.Coffee;

namespace RandomCoffeeServer.Controllers.ApiControllers;

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
        if (HttpContext.GetUserId() is not { } userId)
            throw new InvalidProgramException();

        var groupId = Guid.NewGuid();

        await groupService.AddGroup(
            new Group
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

        var groupIds = await groupService.GetGroupsByUser(userId);
        return Ok(groupIds);
    }

    [HttpGet("{groupId:guid}")]
    public async Task<ActionResult> Get(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var group = await groupService.GetGroupWithParticipants(groupId);
        if (group is null)
            return NotFound();

        return Ok(group);
    }

    [HttpPost("{groupId:guid}/join")]
    public async Task<IActionResult> Join(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        if (HttpContext.GetUserId() is not { } userId)
            throw new InvalidProgramException();

        await groupService.AddUserToGroup(userId, groupId);
        return Ok();
    }

    private readonly GroupService groupService;
}