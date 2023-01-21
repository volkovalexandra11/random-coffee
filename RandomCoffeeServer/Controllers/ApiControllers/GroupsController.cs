using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomCoffeeServer.Controllers.ApiControllers.GroupsControllerDtos;
using RandomCoffeeServer.Domain.Dtos;
using RandomCoffeeServer.Domain.Models;
using RandomCoffeeServer.Domain.Services.Coffee;
using RandomCoffeeServer.Domain.Services.Coffee.Rounds;

namespace RandomCoffeeServer.Controllers.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    public GroupsController(
        GroupService groupRepository,
        GroupRoundMakerService roundMakerService)
    {
        this.groupService = groupRepository;
        this.roundMakerService = roundMakerService;
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

        var (group, participants) = await groupService.GetGroupWithParticipantModels(groupId);
        if (group is null)
            return NotFound();

        var participantsAsDto = participants!.Select(
                user => new ParticipantDto
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePictureUrl = user.ProfilePictureUrl
                })
            .ToList();

        var groupWithParticipants = new GroupWithParticipantsDto
        {
            GroupId = group.GroupId,
            Admin = participantsAsDto.First(participant => participant.UserId == group.AdminUserId),
            Name = group.Name,
            IsPrivate = group.IsPrivate,
            Participants = participantsAsDto,
            NextRoundDate = DateTime.Now
        };


        return Ok(groupWithParticipants);
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

    [Authorize]
    [HttpPost("{groupId:guid}/make-round")]
    public async Task<IActionResult> Start(Guid groupId)
    {
        if (groupId == Guid.Empty)
            return BadRequest();

        var group = await groupService.GetGroup(groupId);

        if (group is null)
            return NotFound();

        var userId = HttpContext.GetUserId();
        if (userId != group.AdminUserId)
        {
            return Unauthorized();
        }

        await roundMakerService.MakeRound(groupId);
        return Ok();
    }

    private readonly GroupService groupService;
    private readonly GroupRoundMakerService roundMakerService;
}